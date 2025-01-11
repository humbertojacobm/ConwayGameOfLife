# ConwayGameOfLife

## Overview
ConwayGameOfLife is a project that implements Conway's Game of Life following the principles of Clean Architecture. This repository is designed to be scalable and adaptable, making it easy to deploy on cloud platforms such as AWS Elastic Beanstalk or Azure App Services. The project also includes Docker support for containerization and horizontal scalability.

---

## Technology Stack

The following technologies are used in this project:

- **Asynchronous Programming** (to handle I/O-bound latency effectively)
- **Native Dependency Injection** (provided by ASP.NET Core Web API)
- **Fluent Validation**
- **AutoMapper**
- **.NET 8.0**
- **C#.NET**
- **ASP.NET Core Web API**
- **SQL Server**
- **Entity Framework**
- **NUnit**
- **Moq**
- **Docker**
- **Docker Swarm**
- **Swagger**
- **Postman**

---

## Architecture

This project follows the principles of **Clean Architecture**, which allows for flexibility and scalability in the application design. The architecture is structured in such a way that:

- The **core business logic** is independent of external dependencies.
- The **infrastructure layer** can be swapped to use different implementations (e.g., switching from SQL Server to a semi-structured database like Amazon DynamoDB).
- The design enables easy testing and maintainability of code.

### Deployment Capabilities

This repository is configured to support deployment on:

1. **AWS Elastic Beanstalk**
2. **Azure App Services**

Both deployment options use Docker Registry for container management, ensuring scalability and reliability.

---

## Scalability

This application is built with scalability in mind:

- **Horizontal Scaling**: Containers can be scaled horizontally using Azure App Services or AWS Elastic Beanstalk.
- **Future Enhancements**: The project is prepared for additional features, including:
  - **Docker Compose**: As more services are added, Docker Compose can be used to manage multiple containers.
  - **API Gateway/Reverse Proxy**: Implemented using NGINX for routing and load balancing.
  - **Message Broker Pattern**: RabbitMQ can be integrated for inter-service communication when needed.
  - **Caching**: Performance can be enhanced using Redis as a caching layer.

---

## Features and Improvements

This repository demonstrates the flexibility of Clean Architecture by enabling:

- **Nested Loops Analysis**: The application contains parts with nested loops of two levels. We are exploring whether serverless functions, such as AWS Lambda or Azure Durable Functions, could optimize these operations for better scalability and performance.

- **Parallel Programming**: The `main` branch includes an enhancement using parallel programming to address CPU-bound latency in the processing of Conway's rules.

---

## Postman Collection

A Postman collection named `ConwaGameOfLife.postman_collection.json` is included in the repository. This collection can be used to test the API if the entire architecture is deployed locally by the developer.

---

1. **Database Flexibility**: Swap the infrastructure layer to use a different database technology, such as Amazon DynamoDB.
2. **Cloud-Ready Design**: Deployable on major cloud platforms with built-in support for scaling.
3. **Extensible Microservices**: Future-proofed for multi-service architecture with API Gateway and Docker Compose.
4. **Performance Optimization**: Ready to integrate Redis for caching and RabbitMQ for message queuing.

---


## Contributing

Contributions are welcome! Feel free to fork the repository, make improvements, and submit a pull request.

---

