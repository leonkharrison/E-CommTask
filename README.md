# E-Commerce Task
# Overview

This technical task was completed as part of the interview process for Victorian Plumbing.

The solution contains two projects, a webApi to inteface with a client front-end, and a "data" project that hold general data models that are used.

The main order placing endpoint is POST api/orders.
This endpoint takes a string for the name of the ordering customer and a list of IDs for each product to order.

I made the decision to only include the Ids in this request as the other information wasnt necessary as its information the back-end already has access to.
It was assumed that any front-end client using this back-end would first call GET api/products, to load a list of available products for the user to choose from. 
The Ids of the selected products can then be sent to place the order.

I have also included a couple of endpoints to view orders ( for demonstration purposes ) GET api/orders and GET api/orders/{id:int}.
These can be used to view the orders once they have been placed.

I have also included an endpoint for adding new products, POST api/products, again, just for demonstration purposes.

When an order is placed the list is checked against a rudimentary cache of products that is used in this task for demonstration.

This project uses a SQLite database and EntityFramwork as the ORM. DB file is placed in the directory to make it clear to the reader. In practice this would be somewhere more secure ( user data, appdata etc for local ) 

# Improvements / Next steps
Some notes on where I would go next with a project such as this
- Improved caching of data to make it thread-safe and move to an actual cache, at the moment its being refreshed everytime the products service is accessed which is unnecessary 
- Auth to allow a users order to be stored against personal details so they can view their order at another time
  - Including guest tokens and orders stored against a name ( like now ) so that guests can also view their order
- At the moment its possible to order multiple of the same prduct, which is fine but it saves each product as a new ProductOrder which isnt very efficient.
  This can be improved by adding a "count" field to this table which stores the amount of the same product in a single order
