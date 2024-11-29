import { Title } from "@solidjs/meta";
import { HttpStatusCode } from "@solidjs/start";

export default function NotFound() {
  return (
    <main>
      <Title>Not Found</Title>
      <HttpStatusCode code={404} />
      <h1>Page Non trouvée</h1>
      <p>
        Cette page n'existe pas ou a été déplacée. Retournez à la page d'accueil ou <a href="/">cliquez ici</a> pour revenir à la page d'accueil.
      </p>
    </main> 
  );
}
