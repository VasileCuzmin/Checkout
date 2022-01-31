
**Techology stack**: .NET Core 5.0, EFCore, Nats streaming, MSSQL

**Test coverage**: 39.65%

![image](https://user-images.githubusercontent.com/40390118/151759559-1a1abc36-b069-47d5-a8da-d75ae2196291.png)

I choose to go with some DDD practices like aggregates and entities, invariants, factories and event sourcing. I used building blocks(abstractions) from https://github.com/VCuzmin/nbb.

1. Setup your connetion to the DB in connection string from Checkout.Migrations, Checnkout.Api and Checkout.Worker
2. Run the migrations project
3. Run these commands for setting up the nats streaming server (make sunre to have Docker For Desktop locally!)

      ```
      docker pull nats-streaming:latest
      docker run -p 4222:4222 nats-streaming -cid faas-cluster
      ```
      
 4. Start the api and the worker
 5. Open the Swagger page through http://localhost:55555/swagger
 6. Open **Checkout.postman_collection.json** in Postman

Enjoy!!
