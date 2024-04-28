# CDS
Content Distribution Service

## Project Description
Build a service that aggregates data from 4 other external services and exposes an API interface to consumers in order to provide them with the content and its metadata.

## Definitions:
- Asset: A file being created by the creative department. We will be referring to it as content or file.
- Briefing: The information around the creative owner of an asset
- Content Distribution: The information around the retrieval method of an asset.
- Order: The information around the placed orders on assets

## Ecosystem
The 4 external services (which belong to different domains) will be mocked in this project as microservices:
- Asset domain : This domain produces a message on a sqs whenever an asset is being uploaded
- Briefing Domain: This domain provides an API in order to retrieve briefing metadata for an asset
- Content Distribution Domain: This domain produces a message on a sqs whenever a new content distribution file is available
- Orders domain: This domain produces a message on a sqs whenever an order is placed

// Add graph of ecosystem

## Mocking external domains
### Deployment
Each of the external domains will be simulated with a microservice deployed on its own container
### Data
Each of the external domains will initialize a repository to hold the sample data from the json files provided.
- Asset domain: will initialize the Assets repository with the data provided in the `AssetMetadata.json`
- Briefing domain: will initialize the Briefings repository with the data provided in the `BriefingMetadata.json`
- Content Distribution Domain: will use the data provided in the `ContentDistributionMetadata.json`
- Orders domain: will use the data provided in the `OrderListMetadata.json`
### Interactions with the CDS
In order to emulate the functionalities of the external domains I will expose an API in each to simulate actions from those mocked services.
- Asset domain:
  - POST `/assets/:id` this endpoint will simulate a message being published to the sqs for an asset that was created
- Briefing domain:
  - GET `/briefings/:name` the endpoint will simulate the exposed API that we assume the Briefing domain would have in order to retrieve a briefing by name
- Content Distribution Domain:
  - POST `/content-distributions/original` this endpoint will simulate a message being published to the sqs for a new content distribution
- Orders domain:
  - POST `/orders/original` this endpoint will simulate a message being published to the sqs for an order that was placed
  - POST `/orders/random` this endpoint will publish a message in the sqs for an order with randomized data

// TODO Add container diagram of the services interactions

## CDS Aggregator
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
handle the polling for each queue and when it consumes a message it will make a callback to the SqsConsumerService it belongs.

The aggregated data will be passed from the SqsConsumerServices to the AggregatorService in order to create database objects for the receiving asset metadata.

## CDS Caching of content distribution
In order to have quick access to the url of the content distribution for each file, the metadata from the content distribution will be cached in a no-sql db
This way when the consumers (digital platforms) request the file from our service, we will be able to redirect them to the file url without even needing to access the database.
Optimising this way a lot the speed of the response, and minimizing the load to the system.

The content distribution file when it arrives to the service through SQS it contains a distribution date. 
This date will be used to cache the latest, based on the distribution date, url for each file.

## CDS API
- GET `/assets` : returns the list of available assets
- GET `/assets/:id` : redirects the user to the fie url corresponding to the latest content distribution
- GET `/assets/:id/metadata`: will return the metadata of the asset

## CDS Modules

// Add class diagram of the various modules (API, Aggregators, DB, Adapters)

## Solution

The final solution can be summarized as:
- A service (CDS) that aggregates data from 4 external domains via SQS and REST API. 
- Using the data to create a complete asset object in its internal DB
- Caching the content distribution data (file url) for fast delivery
- Exposes an API for the consumers (digital platforms) to be able to list and get the assets and their metadata.
- Use of DDD (Domain Driven Design) internally of the CDS service with adapters for the integration with the external domains.
- Use of clean architecture to separate the CDS modules to avoid unnecessary dependencies between them.

// Add graph context and container graph of the solution

## Future improvements
- S3 & DNS
- CI/CD
- Auth system
- Use order data to identify most requested assets and serve them better

// Add container graph including future improvements

