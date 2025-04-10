"use client";

import { useEffect } from "react";
import * as productServices from "~/services/catalog.service";

const ProductGridViewPage = () => {
  const getAllProductAsync = async () => {
    const res = await productServices.getAllProductAsync();
    console.log(res);
  };

  useEffect(() => {
    getAllProductAsync();
  }, []);

  return (
    <div>
      <h1>product page</h1>
    </div>
  );
};

export default ProductGridViewPage;
