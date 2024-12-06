/* eslint-disable react/no-unescaped-entities */
import CardWrapper, {
  AppCardProps,
  CardContent,
} from "~/components/card-wrapper";
import { AreaChartInteractive } from "~/components/chart-ui/area-chart-interactive";
import { BarChartInteractive } from "~/components/chart-ui/bar-chart-interactive";
import { BarChartLabel } from "~/components/chart-ui/bar-chart-label";

const CardData: AppCardProps[] = [
  {
    label: "Total Revenue",
    amount: "+5231",
    description: "+20.1% from last month",
    icon: "applications",
  },
  {
    label: "Users",
    amount: "+600",
    description: "+201 since last hour",
    icon: "shortlisted",
  },
  {
    label: "Sales",
    amount: "+500",
    description: "+19% from last month",
    icon: "rejected",
  },
  {
    label: "Product active",
    amount: "+10",
    description: "Products are active",
    icon: "on-hold",
  },
];

const RevenueAnalyticsPage = () => {
  return (
    <div className="flex flex-col flex-1 gap-4 p-4">
      <CardContent>
        <h2 className="ml-5 text-2xl font-bold mb-3">Revenue Report</h2>

        <div className="flex gap-4">
          <CardWrapper
            label={CardData[0].label}
            amount={CardData[0].amount}
            description={CardData[0].description}
            icon={CardData[0].icon}
          />

          <CardWrapper
            label={CardData[1].label}
            amount={CardData[1].amount}
            description={CardData[1].description}
            icon={CardData[1].icon}
          />

          <CardWrapper
            label={CardData[2].label}
            amount={CardData[2].amount}
            description={CardData[2].description}
            icon={CardData[2].icon}
          />

          <CardWrapper
            label={CardData[3].label}
            amount={CardData[3].amount}
            description={CardData[3].description}
            icon={CardData[3].icon}
          />
        </div>
      </CardContent>

      <CardContent>
        <BarChartLabel />
      </CardContent>

      <CardContent>
        <BarChartInteractive />
      </CardContent>

      <CardContent>
        <AreaChartInteractive />
      </CardContent>
    </div>
  );
};

export default RevenueAnalyticsPage;
