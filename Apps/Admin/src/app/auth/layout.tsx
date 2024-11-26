const AuthLayout = ({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) => {
  return (
    <div>
      <h1>Auth layout</h1>

      {children}
    </div>
  );
};

export default AuthLayout;
