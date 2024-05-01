# CDS
#### Content Distribution Service

### Project Description
#### Build a service that aggregates data from 4 other external services and exposes an API interface to consumers in order to provide them with the content and its metadata.

### Definitions:
- **Asset**: A file being created by the creative department. We will be referring to it as content or file.
- **Briefing**: The information around the creative owner of an asset
- **Content Distribution**: The information around the retrieval method of an asset.
- **Order**: The information around the placed orders on assets

### Ecosystem
The 4 external services (which belong to different domains) will be mocked in this project as microservices:
- **Asset domain**: This domain produces a message on a sqs whenever an asset is being uploaded
- **Briefing Domain**: This domain provides an API in order to retrieve briefing metadata for an asset
- **Content Distribution Domain**: This domain produces a message on a sqs whenever a new content distribution file is available
- **Orders domain**: This domain produces a message on a sqs whenever an order is placed

// Add graph of ecosystem

### Design and Architecture

This project leverages Domain-Driven Design (DDD) and Clean Code Architecture to provide a robust, scalable, and adaptable solution.
Knowing its size might not qualify for such an approach, this choice demonstrates a commitment to well-structured design principles.
The service utilizes loosely-coupled adapters for external interactions, ensuring flexibility and resilience. 
If an external system changes (e.g., a content distribution domain switches from SQS to a REST API), only a single adapter needs to be updated, minimizing overall impact.

### Structure

The project is structured as follows:

- `CDS.API`: The API layer of the application, serving as the entry point for the digital platform consumers.
- `CDS.Application`: Application logic and service layer that contains business rules.
- `CDS.Domain`: Core domain logic and entities.
- `CDS.Infrastructure`: Infrastructure setup including databases, sqs consumers, http clients and caching.
- `Mock.*`: Mock implementations for various parts of the system such as Briefing, Order, Content Distribution, and Assets.

### Mocking external domains

#### Deployment
Each of the external domains will be simulated with a microservice deployed on its own container

#### Data
Each of the external domains will initialize a repository to hold the sample data from the json files provided.
- **Asset domain**: will initialize the Assets repository with the data provided in the `AssetMetadata.json`
- **Briefing domain**: will initialize the Briefings repository with the data provided in the `BriefingMetadata.json`
- **Content Distribution Domain**: will use the data provided in the `ContentDistributionMetadata.json`
- **Orders domain**: will use the data provided in the `OrderListMetadata.json`

#### Interactions with the CDS
In order to emulate the functionalities of the external domains I will expose an API in each service to simulate actions from those mocked domains.
- Asset domain:
  - POST `/assets/:id` this endpoint will simulate a message being published to the sqs for an asset that was created
  - POST `/assets/all` this endpoint will simulate messages being published to the sqs for all the assets that was created at once
- Briefing domain:
  - GET `/briefings/:name` this endpoint will simulate the exposed API that we assume the Briefing domain would have in order to retrieve a briefing by name
  - GET `/briefings` this endpoint will simulate the exposed API that we assume the Briefing domain would have in order to retrieve all briefings
- Content Distribution Domain:
  - POST `/content-distributions/original` this endpoint will simulate a message being published to the sqs for a new content distribution
- Orders domain:
  - POST `/orders/original` this endpoint will simulate a message being published to the sqs for an order that was placed
  - POST `/orders/random` this endpoint will publish a message in the sqs for an order with randomized data

// TODO Add container diagram of the services interactions

### CDS Aggregator
The service will aggregate data from the 4 external domains. 
In order to do that it will consume messages from the SQS queues from the:
- Asset domain
- Orders domain
- Content Distribution Domain

And it will receive data through a REST API from the:
- Briefing domain

In order to achieve the SQS polling for the 3 domains without code duplication, an Interface will be implemented (ISqsConsumerService).

Those SqsConsumerServices will be instantiated as IHostedServices in order to run in the background performing the polling.

In order to avoid code duplication for the polling, a class (SqsPoller) will be instantiated by each SqsConsumerServices which will
handle the polling for each queue and when it consumes a message it will make a callback to the belonging SqsConsumerService.

The aggregated data will be passed from the Infrastructure layer where the SqsConsumerServices belong, to the Application layer in order for the domain objects to be created and persisted in the database.

When we receive an Asset from the AssetDomain, while the Application layer creates the object we will reach out to the Briefing domain to collect the briefing metadata and persist them in parallel with the asset.

### CDS Caching of content distribution
In order to have quick access to the url of the content distribution for each file, the metadata from the content distribution will be cached in a no-sql db
This way when the consumers (digital platforms) request the file from our service, we will be able to redirect them to the file url without even needing to access the database.
Optimising this way a lot the speed of the response, and minimizing the load to the system.

The content distribution file when it arrives to the service through SQS it contains a distribution date, which will be used to cache the latest based on the distribution date url for each file.

### CDS API
- GET `/assets` : returns the list of available assets
- GET `/assets/:id` : redirects the user to the fie url corresponding to the latest content distribution
- GET `/assets/:id/metadata`: returns the metadata of the asset

### Solution

#### Summary
The final solution can be summarized as:
- A service (CDS) that aggregates data from 4 external domains via SQS and REST API. 
- Using the data to create a complete asset object persisted in its internal DB.
- Caching the content distribution data (file url) for fast delivery.
- Exposes an API for the consumers (digital platforms) to be able to list and get the assets and their metadata.
- Use of DDD (Domain Driven Design) to design the CDS service with adapters for the integration with the external domains.
- Use of Clean Architecture to separate the CDS layers to avoid unnecessary dependencies between them.

#### Technologies used

- Docker (containerization)
- Redis (caching)
- Postgres (database)
- LocalStack (mocking AWS services)
- EF.Core (ORM)

#### Known Limitations

- If the briefing is not available the time an asset is consumed by the CDS service, the asset will be persisted without the briefing metadata.
- Receiving an order that contain assets that don't exist, will produce orphaned AssetOrder objects in the database.
- Receiving a content distribution that contain assets that don't exist, will produce orphaned AssetContentDistribution objects in the database and unused cache entries.

// Add graph context and container graph of the solution

### Future improvements
- S3 & DNS
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
- Use the postman collection to interact with the mocked domains in order to add data to teh CDS service
- Use the CDS API to get the assets and their metadata

#### DB Migrations
To create the migrations run:
```bash
dotnet ef migrations add CreateTables -p CDS.Infrastructure -s CDS.API
```

Migrations are applied automatically during the service start up.

To manually apply the migrations if needed having a local db running in a container run:
```bash
dotnet ef database update -p CDS.Infrastructure -s CDS.API --connection "Host=localhost:5432;Database=cds;Username=cds;Password=cds"
```