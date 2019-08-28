# AssetManagement

Technologies and services used are :

EntityFramework: Managing database interactions,
Azure sql: Managing database,
Azure App service: for publishing the api,
Azure storage (blobs): For storing files.
Azure Functions: when file is uploaded to azure storage azure function is triggered and code writen there is used for further processing of files according to social media platforms facebook and twitter,
Azure cognitive services: For analysing the files.
Video Indexer can be used for getting metadata about image but its not implemented because of seperate portal than azure.

All the related requests are in ValuesController. User Authentication is managed by dotnet Identity server.
