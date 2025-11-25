/* eslint-disable react-hooks/exhaustive-deps */
import { useEffect, useRef } from 'react';
import isEqual from 'lodash/fp/isEqual';

/**
 * This hook can be helpful in situations where the dependencies are complex objects or arrays,
 * and you want to ensure that the effect only runs when the specific values inside the dependencies have changed.
 * It can help prevent unnecessary re-renders and improve performance.
 * @param callback - The callback to run on effect
 * @param dependencies - The dependencies to watch
 * @returns void
 */
const useDeepCompareEffect = (callback: () => void, dependencies: any) => {
   const currentDependenciesRef = useRef<typeof dependencies>(dependencies);

   if (!isEqual(currentDependenciesRef.current, dependencies)) {
      currentDependenciesRef.current = dependencies;
   }

   useEffect(callback, [currentDependenciesRef.current]);
};

export default useDeepCompareEffect;
