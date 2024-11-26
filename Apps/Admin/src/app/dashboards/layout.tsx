const DashboardLayout = ({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) => {
  return (
    <div>
      <h1>Dashboard layout</h1>

      {children}
    </div>
  );
};

export default DashboardLayout;
