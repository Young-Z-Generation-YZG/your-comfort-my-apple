import { redirect } from 'next/navigation';

export default function Home() {
   // Use permanent redirect to avoid redirect loops
   redirect('/dashboard/revenue-analytics');
}
