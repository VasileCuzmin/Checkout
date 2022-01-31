
**Techology stack**: .NET Core 5.0, EFCore, Nats streaming, MSSQL

1. Setup your connetion to the DB in connection string from Checkout.Migrations, Checnkout.Api and Checkout.Worker
2. Run the migrations project
3. Run these commands for setting up the nats streaming server (make sunre to have Docker For Desktop locally!)

      ```
      docker pull nats-streaming:latest
      docker run -p 4222:4222 nats-streaming -cid faas-cluster
      ```
      
 4. Start the api and the worker! 

Enjoy!!
