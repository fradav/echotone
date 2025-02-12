import * as index from "./index.css";
import { App } from "./App.jsx";
import { render } from "solid-js/web";


export function Root() {
    return <>
        {App()}
    </>;
}

render(Root, document.getElementById("root"));

