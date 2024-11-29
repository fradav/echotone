import { createSignal } from "solid-js";
import { Button } from "@kobalte/core/button";
import "./Counter.scss";

export default function Counter() {
  const [count, setCount] = createSignal(0);
  return (
    <Button class="increment" onClick={() => setCount(count() + 1)} type="button">
      Clicks: {count()}
    </Button>
  );
}
