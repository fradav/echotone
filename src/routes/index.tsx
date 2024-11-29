import { Title } from "@solidjs/meta";
// import Counter from "~/components/Counter";
import GridMasonry from "~/components/GridMasonry";
import SolidMasonry from "~/components/SolidMasonry";

export default function Home() {
  return (
    <main>
      <Title>Échotone</Title>
      {/* <h1>Échotone</h1> */}
      <SolidMasonry />
    </main>
  );
}
