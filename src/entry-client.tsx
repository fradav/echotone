// @refresh reload
import { mount, StartClient } from "@solidjs/start/client";

const start = mount(() => <StartClient />, document.getElementById("app")!);

export default start;