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
- Content Distribution Domain: This domain populates a cache with the current distribution details of the assets
- Orders domain: This domain produces a message on a sqs whenever an order is placed

## Mocking external domains
### Deployment
Each of the external domains will be simulated with a microservice deployed on its own container
### Data
Each of the external domains will initialize a repository, cache or database to hold the sample data from the json files provided.
- Asset domain: will initialize the Assets repository with the data provided in the `AssetMetadata.json`
- Briefing domain: will initialize the Briefings database with the data provided in the `BriefingMetadata.json`
- Content Distribution Domain: will populate a cache with the data provided in the `ContentDistributionMetadata.json`
- Orders domain: will initialize the Orders repository with the data provided in the `OrderListMetadata.json`
### Interactions with the CDS
In order to emulate the functionalities of the external domains I will expose an API in each to simulate actions from those mocked services.
- Asset domain:
  - POST `/assets/:id` this endpoint will simulate a message being push to the sqs for an asset that was created
- Briefing domain:
    - POST `/db/initialize` this endpoint will initialize the Briefings database
    - GET `/briefings/:name` the endpoint will simulate the exposed API that we assume the Briefing domain would have in order to retrieve a briefing by name
- Content Distribution Domain:
  - POST `/cache` this endpoint will simulate the internal process of the Content Distribution Domain to 
- Orders domain:
  - POST `/orders/:orderNumber` this endpoint will simulate a message being push to the sqs for an order that was placed
  - POST `/orders/random` this endpoint will create a message in the sqs for an order with randomized data

