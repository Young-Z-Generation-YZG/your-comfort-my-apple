import { ModeToggle } from "~/components/mode-toggle";
import { Button } from "~/components/ui/button";

const LoginPage = () => {
  return (
    <div>
      <h1>Login page</h1>
      <Button>Log in</Button>
      <ModeToggle />
    </div>
  );
};

export default LoginPage;
