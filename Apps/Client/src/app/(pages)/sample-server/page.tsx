/* eslint-disable react/react-in-jsx-scope */

import { getPosts } from '~/services/example/post.service';

const Test = async () => {
   const data = await getPosts();

   console.log(data);

   return (
      <div>
         <h1>Server call</h1>
      </div>
   );
};

export default Test;
