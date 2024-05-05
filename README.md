# CDS
#### Content Distribution Service

### Project Description
#### Build a service that aggregates data about assets from 4 external services and exposes an API interface to consumers in order to provide them with the asset content and metadata.

### Summary
The final solution can be summarized as:
- A service (CDS) that aggregates data from 4 external domains via SQS and REST API.
- Using the data to create a complete asset object persisted in its internal DB.
- Caching the content distribution data (file url) for fast delivery.
- Exposes an API for the consumers (digital platforms) to be able to list and get the assets including all their metadata.
- Use of DDD (Domain Driven Design) principles to closely align with business requirements and ensure scalability.
- Use of Clean Architecture to separate the CDS layers avoiding unnecessary dependencies between them.
- Use elements of Hexagonal Architecture for easily interchangeable adapters with the external services.
- Unit tests
- End-To-End tests

### Definitions:
- **Asset**: A file being created by the creative department. We will be referring to it as content or file.
- **Briefing**: The information around the creative owner of an asset
- **Content Distribution**: The information around the retrieval method of assets.
- **Order**: The information around the placed orders on assets

### Ecosystem
The 4 external services (which belong to different domains) will be mocked in this project as microservices:
- **Asset domain**: We will assume that this domain produces a message on a sqs whenever an asset is being uploaded
- **Briefing Domain**:  We will assume that this domain provides an API in order to retrieve briefing metadata for an asset
- **Content Distribution Domain**:  We will assume that this domain produces a message on a sqs whenever a new content distribution file is available
- **Orders domain**:  We will assume that this domain produces a message on a sqs whenever an order is placed

// Add graph of ecosystem

### Design and Architecture

This project leverages Domain-Driven Design (DDD) and Clean Code Architecture to provide a robust, scalable, and adaptable solution.
Knowing its size might not qualify for such an approach, this choice demonstrates a commitment to well-structured design principles.
The service utilizes loosely-coupled adapters for external interactions, ensuring flexibility and resilience. 
If an external system changes (e.g. the content distribution domain switches from SQS to a REST API), only a single adapter needs to be updated, minimizing overall impact on the system.

### Structure

The project is structured as follows:

- `CDS.API`: The API layer of the application, serving as the entry point for the digital platform consumers.
- `CDS.Application`: Application logic and service layer that contains business rules.
- `CDS.Contracts`: Contracts between the layers in order to enable decoupling. Interfaces and DTOs are defined here.
- `CDS.Domain`: Core domain entities with invariants.
- `CDS.Infrastructure`: Infrastructure setup including databases, sqs consumers, http clients and caching.
- `Mock.*`: Mock implementations / microservices for the 4 external domains such as Briefing, Order, Content Distribution, and Asset.

### Mocking external domains

#### Deployment
Each of the external domains will be simulated with a microservice deployed on its own container

#### Data
Each of the external domains will initialize a repository to hold the sample data from the json files provided.
- **Asset domain**: will initialize the Assets repository with the data provided in the `AssetMetadata.json`
- **Briefing domain**: will initialize the Briefings repository with the data provided in the `BriefingMetadata.json`
- **Content Distribution Domain**: will use the data provided in the `ContentDistributionMetadata.json`
- **Orders domain**: will use the data provided in the `OrderListMetadata.json`

#### Interaction with the CDS
In order to emulate the functionalities of the external domains I will expose an API in each service to simulate actions from those mocked domains.
- Asset domain:
  - POST `/assets/:id` this endpoint will trigger a message to be published on the sqs for an asset that was created
  - POST `/assets/all` this endpoint will trigger messages to be published on the sqs for all the assets that were created at once
- Briefing domain:
  - GET `/briefings/:name` this endpoint will simulate the exposed API that we assume the Briefing domain would have in order to retrieve a briefing by name
  - GET `/briefings` this endpoint will simulate the exposed API that we assume the Briefing domain would have in order to retrieve all briefings
- Content Distribution Domain:
  - POST `/content-distributions/original` this endpoint will trigger a message to be published on the sqs for a new content distribution
- Orders domain:
  - POST `/orders/original` this endpoint will trigger a message to be published on the sqs for an order that was placed
  - POST `/orders/random` this endpoint will trigger a message to be published on the sqs for an order with randomized data

// TODO Add container diagram of the services interactions

### CDS Aggregator
The service will aggregate data from the 4 external domains. 
In order to do that it will consume messages from the SQS queues from the:
- Asset domain
- Orders domain
- Content Distribution Domain

And it will request data through a REST API from the:
- Briefing domain

In order to achieve the SQS polling for the 3 domains without code duplication, an Interface will be implemented (ISqsConsumerService).

Those SqsConsumerServices will be instantiated as IHostedServices in order to run in the background performing the polling.

In order to avoid code duplication for the polling, a class (SqsPoller) will be instantiated by each SqsConsumerServices which will
handle the polling for each queue. When it consumes a message it will make a callback to the belonging SqsConsumerService.

The aggregated data will be passed from the Infrastructure layer where the SqsConsumerServices belong, to the Application layer in order to be mapped to domain entities and to be persisted in the database.

When we receive an Asset from the AssetDomain, while the Application layer creates the object, it will reach out to the Briefing domain to collect the briefing metadata and persist it in parallel with the asset.

When we receive an Order from the OrderDomain, the Application layer will create the order object and one entity object for each related asset in the order.

When we receive a Content Distribution from the ContentDistributionDomain, the Application layer will create the content distribution object and one entity object for each related asset in the content distribution.

### CDS Caching of content distribution
In order to have quick access to the url of the content distribution for each file, the metadata from the content distribution will be cached in a no-sql db
This way when the consumers (digital platforms) request the file from our service, we will be able to redirect them to the file url without even needing to access the database.
Optimizing this way a lot the speed of the response, and minimizing the load to the system.

The content distribution file, when it arrives at the service through SQS, contains a distribution date, which will be used to cache the latest based on the distribution date url for each file.

A passive cache repopulation mechanism is also in place. If for any reason the cache is not available, the service will fetch the data from the database and cache it for future requests.

### CDS API
- GET `/assets` : returns a paginated list of available assets
- GET `/assets/:id` : redirects the user to the file url corresponding to the latest content distribution
- GET `/assets/:id/metadata`: returns the metadata of the asset

#### For testing purposes
- DELETE `/admin/reset` : Empty the database and purge the cache.
- DELETE `/admin/drop-db` : Empties the database.
- DELETE `/admin/purge-cache` : Purges the cache.

### Solution

// Add context graph of the solution

#### Technologies used

- .Net 8
- Docker (containerization)
- Redis (caching)
- Postgres (database)
- LocalStack (mocking AWS services)
- EF.Core (ORM)

#### Known Limitations

- Receiving an order that contains assets that don't exist, will produce orphaned AssetOrder objects in the database.
- Receiving a content distribution that contains assets that don't exist, will produce orphaned AssetContentDistribution objects in the database and unused cache entries.
- The asset metadata list for orders and content distribution is not paginated. This could lead to performance issues if the list grows too large.
- If an asset's briefing is unavailable when an asset is consumed, the asset will be persisted without the briefing metadata.

### Future improvements
- S3 & CDN
- CI/CD
- Auth system
- Use order data to identify most requested assets and serve them better

// Add container graph including future improvements

### Technical details

#### Run the project
- Make sure docker is installed and running in your system
- Navigate to the project directory
- Run the command 
```bash
docker-compose up
```

#### Use the project
- Use the postman collections and environment to interact with the mocked domains in order to add data to the CDS service
- Use the CDS API to get the assets and their metadata

### Test the project
#### Unit tests

To run the unit tests navigate to the project directory and run:
```bash
dotnet test
```

#### End-to-End tests

To run the end-to-end tests:
- Use postman with the provided 'End-to-End Tests' collection and the 'local' environment.
- Run the collections using postman runner 

Note: The testing scenarios in the collection have defined postman scripts to verify the expected behaviors.

#### DB Migrations
To create new migrations in the future make sure you have installed: 
- .NET SDK 8
- Add the Entity Framework Core package
- install EF Core Tools

then navigate to the project directory and run:
```bash
dotnet ef migrations add CreateTables -p CDS.Infrastructure -s CDS.API
```

Note: Migrations are applied automatically during the service start up.

To manually apply the migrations if needed having a local db running, run:
```bash
dotnet ef database update -p CDS.Infrastructure -s CDS.API --connection "Host=localhost:5432;Database=cds;Username=cds;Password=cds"
```