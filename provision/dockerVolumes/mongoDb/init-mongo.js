db = db.getSiblingDB("admin");
db.auth("bale", "bale");

catalogDb = db.getSiblingDB("CatalogDb");

catalogDb.createCollection("Categories");

// db.createCollection("Products");
// db.createCollection("ProductItems");

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

// db.createCollection("test_docker");
