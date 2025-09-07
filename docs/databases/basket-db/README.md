# Basket Database Documentation

## Overview

The basket database stores shopping cart information for the e-commerce system.

## Schema

### Table: mt_doc_shoppingcart

- **id**: varchar (Primary Key) - Unique identifier for the shopping cart
- **data**: jsonb - Cart data stored as JSON
- **mt_last_modified**: timestamptz - Last modification timestamp
- **mt_version**: uuid - Version identifier for optimistic concurrency
- **mt_dotnet_type**: varchar - .NET type information

## Usage

This database is used by the Basket service to persist shopping cart state across user sessions.

## Related Services

- YGZ.Basket.Api
- YGZ.Basket.Application
- YGZ.Basket.Domain
- YGZ.Basket.Infrastructure
