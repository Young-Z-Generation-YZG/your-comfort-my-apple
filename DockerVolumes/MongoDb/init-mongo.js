db = db.getSiblingDB("admin");
db.auth("bale", "bale");

db = db.getSiblingDB("CatalogDb");

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
