db = db.getSiblingDB("admin");
db.auth("bale", "bale");

db = db.getSiblingDB("CatalogDb");

db.createCollection("Products");
db.createCollection("ProductItems");
db.createCollection("Categories");

// db.createUser({
//   user: "test",
//   pwd: "test",
//   roles: [
//     {
//       role: "readWrite",
//       db: "CatalogDb",
//     },
//   ],
// });

db.createCollection("test_docker");
