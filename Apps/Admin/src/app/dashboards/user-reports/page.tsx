import { CardContent, CardWrapper } from '@components/card-wrapper';
import { Tabs, TabsList, TabsTrigger } from '@components/ui/tabs';

const UserReports = () => {
   return (
      <div className="flex flex-col flex-1 gap-4 p-4">
         <h1 className="text-3xl font-bold tracking-tight">User Reports</h1>

         <p className="text-muted-foreground">
            View your revenue analytics and compare them with the previous
            month.
         </p>

         <CardWrapper className="flex flex-col gap-4">
            <div className="flex items-center">
               <p className="font-medium text-sm mr-2">Overview by</p>
               <Tabs defaultValue="daily" className="w-[400px]">
                  <TabsList>
                     <TabsTrigger value="daily">Daily</TabsTrigger>
                     <TabsTrigger value="weekly">Weekly</TabsTrigger>
                     <TabsTrigger value="monthly">Monthly</TabsTrigger>
                  </TabsList>
               </Tabs>
            </div>
            <div className="grid grid-cols-4 gap-4">
               <CardContent
                  title="Total Users"
                  value={267950}
                  compareValue={12.5}
                  compareValueType="percentage"
                  type="amount"
                  compareText="from last month"
                  icon="up"
                  variantColor="green"
               />

               <CardContent
                  title="New Users Today"
                  value={89.32}
                  compareValue={12.5}
                  type="decimal"
                  compareValueType="percentage"
                  compareText="from last month"
                  icon="up"
                  variantColor="green"
               />

               <CardContent
                  title="Active Users Today"
                  value={2.5}
                  valueSuffix="%"
                  compareValue={12.5}
                  type="percentage"
                  compareValueType="percentage"
                  compareText="from last month"
                  icon="up"
                  variantColor="green"
               />

               <CardContent
                  title="Non-Users"
                  value={3281}
                  compareValue={12.5}
                  compareValueType="percentage"
                  compareText="from last month"
                  type="text"
                  icon="up"
                  variantColor="green"
               />
            </div>
         </CardWrapper>
      </div>
   );
};

export default UserReports;
