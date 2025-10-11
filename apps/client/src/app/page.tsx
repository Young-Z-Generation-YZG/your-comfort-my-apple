import { useState } from "react";

export default function Home() {
  const [count, setCount] = useState(0);
  return (
    <div>
      <h1 className="text-3xl font-black text-blue-400">Hello World</h1>
    </div>
  );
}
