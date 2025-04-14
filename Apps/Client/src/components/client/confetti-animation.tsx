'use client';

import { useEffect, useState } from 'react';
import { motion } from 'framer-motion';

interface ConfettiPiece {
   id: number;
   x: number;
   y: number;
   rotation: number;
   scale: number;
   color: string;
}

export function ConfettiAnimation() {
   const [confetti, setConfetti] = useState<ConfettiPiece[]>([]);

   useEffect(() => {
      // Generate confetti pieces
      const colors = ['#22c55e', '#3b82f6', '#f59e0b', '#ec4899', '#8b5cf6'];
      const pieces: ConfettiPiece[] = [];

      for (let i = 0; i < 100; i++) {
         pieces.push({
            id: i,
            x: Math.random() * 100, // percentage across screen
            y: -20 - Math.random() * 10, // start above the viewport
            rotation: Math.random() * 360,
            scale: 0.5 + Math.random() * 1,
            color: colors[Math.floor(Math.random() * colors.length)],
         });
      }

      setConfetti(pieces);

      // Clean up
      return () => {
         setConfetti([]);
      };
   }, []);

   return (
      <div className="fixed inset-0 pointer-events-none overflow-hidden z-50">
         {confetti.map((piece) => (
            <motion.div
               key={piece.id}
               className="absolute w-3 h-3"
               style={{
                  left: `${piece.x}vw`,
                  top: `${piece.y}vh`,
                  backgroundColor: piece.color,
                  borderRadius: Math.random() > 0.5 ? '50%' : '0%',
               }}
               initial={{
                  y: piece.y,
                  x: `${piece.x}vw`,
                  rotate: 0,
                  opacity: 1,
               }}
               animate={{
                  y: '120vh',
                  x: [
                     `${piece.x}vw`,
                     `${piece.x + (Math.random() * 20 - 10)}vw`,
                     `${piece.x + (Math.random() * 20 - 10)}vw`,
                     `${piece.x + (Math.random() * 20 - 10)}vw`,
                  ],
                  rotate: piece.rotation * (Math.random() > 0.5 ? 1 : -1) * 5,
                  opacity: [1, 1, 0.5, 0],
               }}
               transition={{
                  duration: 4 + Math.random() * 3,
                  ease: [0.23, 0.44, 0.34, 0.99],
                  delay: Math.random() * 1,
               }}
            />
         ))}
      </div>
   );
}
