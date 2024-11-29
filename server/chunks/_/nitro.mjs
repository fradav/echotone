import process from 'node:process';globalThis._importMeta_=globalThis._importMeta_||{url:"file:///_entry.js",env:process.env};import destr from 'file://C:/Users/fradav/Documents/Dev/Sites/echotone-clean/echotone-web/node_modules/destr/dist/index.mjs';
import { getRequestHeader, splitCookiesString, setResponseHeader, setResponseStatus, send, eventHandler, appendResponseHeader, removeResponseHeader, createError, getResponseHeader, H3Event, setHeader, getRequestIP, getResponseStatus, getResponseStatusText, getCookie, setCookie, getRequestURL, getResponseHeaders, getRequestWebStream, sendRedirect, defineEventHandler, handleCacheHeaders, createEvent, fetchWithEvent, isEvent, setHeaders, proxyRequest, createApp, createRouter as createRouter$1, toNodeListener, lazyEventHandler } from 'file://C:/Users/fradav/Documents/Dev/Sites/echotone-clean/echotone-web/node_modules/h3/dist/index.mjs';
import { createHooks } from 'file://C:/Users/fradav/Documents/Dev/Sites/echotone-clean/echotone-web/node_modules/hookable/dist/index.mjs';
import { createFetch as createFetch$1, Headers as Headers$1 } from 'file://C:/Users/fradav/Documents/Dev/Sites/echotone-clean/echotone-web/node_modules/ofetch/dist/node.mjs';
import { createCall, createFetch } from 'file://C:/Users/fradav/Documents/Dev/Sites/echotone-clean/echotone-web/node_modules/unenv/runtime/fetch/index.mjs';
import _olRoFNXOxi from 'file://C:/Users/fradav/Documents/Dev/Sites/echotone-clean/echotone-web/node_modules/vinxi/lib/app-fetch.js';
import _riM5SrWQFq from 'file://C:/Users/fradav/Documents/Dev/Sites/echotone-clean/echotone-web/node_modules/vinxi/lib/app-manifest.js';
import { decodePath, withLeadingSlash, withoutTrailingSlash, parseURL, joinURL, withoutBase, getQuery, withQuery } from 'file://C:/Users/fradav/Documents/Dev/Sites/echotone-clean/echotone-web/node_modules/ufo/dist/index.mjs';
import { promises } from 'node:fs';
import { fileURLToPath } from 'node:url';
import { dirname, resolve } from 'file://C:/Users/fradav/Documents/Dev/Sites/echotone-clean/echotone-web/node_modules/pathe/dist/index.mjs';
import { fromJSON, crossSerializeStream, getCrossReferenceHeader } from 'file://C:/Users/fradav/Documents/Dev/Sites/echotone-clean/echotone-web/node_modules/seroval/dist/esm/production/index.mjs';
import { CustomEventPlugin, DOMExceptionPlugin, EventPlugin, FormDataPlugin, HeadersPlugin, ReadableStreamPlugin, RequestPlugin, ResponsePlugin, URLSearchParamsPlugin, URLPlugin } from 'file://C:/Users/fradav/Documents/Dev/Sites/echotone-clean/echotone-web/node_modules/seroval-plugins/dist/esm/production/web.mjs';
import { sharedConfig, lazy, createComponent, catchError, onCleanup } from 'file://C:/Users/fradav/Documents/Dev/Sites/echotone-clean/echotone-web/node_modules/solid-js/dist/server.js';
import { renderToString, isServer, getRequestEvent, ssrElement, escape, mergeProps, ssr, createComponent as createComponent$1, ssrHydrationKey, NoHydration, ssrAttribute } from 'file://C:/Users/fradav/Documents/Dev/Sites/echotone-clean/echotone-web/node_modules/solid-js/web/dist/server.js';
import { provideRequestEvent } from 'file://C:/Users/fradav/Documents/Dev/Sites/echotone-clean/echotone-web/node_modules/solid-js/web/storage/dist/storage.js';
import { getContext } from 'file://C:/Users/fradav/Documents/Dev/Sites/echotone-clean/echotone-web/node_modules/unctx/dist/index.mjs';
import { AsyncLocalStorage } from 'node:async_hooks';
import { createRouter, toRouteMatcher } from 'file://C:/Users/fradav/Documents/Dev/Sites/echotone-clean/echotone-web/node_modules/radix3/dist/index.mjs';
import { hash } from 'file://C:/Users/fradav/Documents/Dev/Sites/echotone-clean/echotone-web/node_modules/ohash/dist/index.mjs';
import { createStorage, prefixStorage } from 'file://C:/Users/fradav/Documents/Dev/Sites/echotone-clean/echotone-web/node_modules/unstorage/dist/index.mjs';
import unstorage_47drivers_47fs from 'file://C:/Users/fradav/Documents/Dev/Sites/echotone-clean/echotone-web/node_modules/unstorage/drivers/fs.mjs';
import unstorage_47drivers_47fs_45lite from 'file://C:/Users/fradav/Documents/Dev/Sites/echotone-clean/echotone-web/node_modules/unstorage/drivers/fs-lite.mjs';
import { klona } from 'file://C:/Users/fradav/Documents/Dev/Sites/echotone-clean/echotone-web/node_modules/klona/dist/index.mjs';
import defu, { defuFn } from 'file://C:/Users/fradav/Documents/Dev/Sites/echotone-clean/echotone-web/node_modules/defu/dist/defu.mjs';
import { snakeCase } from 'file://C:/Users/fradav/Documents/Dev/Sites/echotone-clean/echotone-web/node_modules/scule/dist/index.mjs';

function hasReqHeader(event, name, includes) {
  const value = getRequestHeader(event, name);
  return value && typeof value === "string" && value.toLowerCase().includes(includes);
}
function isJsonRequest(event) {
  if (hasReqHeader(event, "accept", "text/html")) {
    return false;
  }
  return hasReqHeader(event, "accept", "application/json") || hasReqHeader(event, "user-agent", "curl/") || hasReqHeader(event, "user-agent", "httpie/") || hasReqHeader(event, "sec-fetch-mode", "cors") || event.path.startsWith("/api/") || event.path.endsWith(".json");
}
function normalizeError(error, isDev) {
  const cwd = typeof process.cwd === "function" ? process.cwd() : "/";
  const stack = (error.stack || "").split("\n").splice(1).filter((line) => line.includes("at ")).map((line) => {
    const text = line.replace(cwd + "/", "./").replace("webpack:/", "").replace("file://", "").trim();
    return {
      text,
      internal: line.includes("node_modules") && !line.includes(".cache") || line.includes("internal") || line.includes("new Promise")
    };
  });
  const statusCode = error.statusCode || 500;
  const statusMessage = error.statusMessage ?? (statusCode === 404 ? "Not Found" : "");
  const message = error.unhandled ? "internal server error" : error.message || error.toString();
  return {
    stack,
    statusCode,
    statusMessage,
    message
  };
}
function _captureError(error, type) {
  console.error(`[nitro] [${type}]`, error);
  useNitroApp().captureError(error, { tags: [type] });
}
function trapUnhandledNodeErrors() {
  process.on(
    "unhandledRejection",
    (error) => _captureError(error, "unhandledRejection")
  );
  process.on(
    "uncaughtException",
    (error) => _captureError(error, "uncaughtException")
  );
}
function joinHeaders(value) {
  return Array.isArray(value) ? value.join(", ") : String(value);
}
function normalizeFetchResponse(response) {
  if (!response.headers.has("set-cookie")) {
    return response;
  }
  return new Response(response.body, {
    status: response.status,
    statusText: response.statusText,
    headers: normalizeCookieHeaders(response.headers)
  });
}
function normalizeCookieHeader(header = "") {
  return splitCookiesString(joinHeaders(header));
}
function normalizeCookieHeaders(headers) {
  const outgoingHeaders = new Headers();
  for (const [name, header] of headers) {
    if (name === "set-cookie") {
      for (const cookie of normalizeCookieHeader(header)) {
        outgoingHeaders.append("set-cookie", cookie);
      }
    } else {
      outgoingHeaders.set(name, joinHeaders(header));
    }
  }
  return outgoingHeaders;
}

function defineNitroErrorHandler(handler) {
  return handler;
}
const errorHandler = defineNitroErrorHandler(
  function defaultNitroErrorHandler(error, event) {
    const { stack, statusCode, statusMessage, message } = normalizeError(
      error);
    const errorObject = {
      url: event.path || "",
      statusCode,
      statusMessage,
      message,
      stack: void 0
    };
    if (error.unhandled || error.fatal) {
      const tags = [
        "[nitro]",
        "[request error]",
        error.unhandled && "[unhandled]",
        error.fatal && "[fatal]"
      ].filter(Boolean).join(" ");
      console.error(
        tags,
        error.message + "\n" + stack.map((l) => "  " + l.text).join("  \n")
      );
    }
    if (statusCode === 404) {
      setResponseHeader(event, "Cache-Control", "no-cache");
    }
    setResponseStatus(event, statusCode, statusMessage);
    if (isJsonRequest(event)) {
      setResponseHeader(event, "Content-Type", "application/json");
      return send(event, JSON.stringify(errorObject));
    }
    setResponseHeader(event, "Content-Type", "text/html");
    return send(event, renderHTMLError(errorObject));
  }
);
function renderHTMLError(error) {
  const statusCode = error.statusCode || 500;
  const statusMessage = error.statusMessage || "Request Error";
  return `<!DOCTYPE html>
  <html lang="en">
  <head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>${statusCode} ${statusMessage}</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/@picocss/pico/css/pico.min.css">
  </head>
  <body>
    <main class="container">
      <dialog open>
        <article>
          <header>
            <h2>${statusCode} ${statusMessage}</h2>
          </header>
          <code>
            ${error.message}<br><br>
            ${"\n" + (error.stack || []).map((i) => `&nbsp;&nbsp;${i}`).join("<br>")}
          </code>
          <footer>
            <a href="/" onclick="event.preventDefault();history.back();">Go Back</a>
          </footer>
        </article>
      </dialog>
    </main>
  </body>
</html>
`;
}

const appConfig$1 = {"name":"vinxi","routers":[{"name":"public","type":"static","base":"/","dir":"./public","root":"C:\\Users\\fradav\\Documents\\Dev\\Sites\\echotone-clean\\echotone-web","order":0,"outDir":"C:/Users/fradav/Documents/Dev/Sites/echotone-clean/echotone-web/.vinxi/build/public"},{"name":"ssr","type":"http","link":{"client":"client"},"handler":"src/entry-server.tsx","extensions":["js","jsx","ts","tsx"],"target":"server","root":"C:\\Users\\fradav\\Documents\\Dev\\Sites\\echotone-clean\\echotone-web","base":"/","outDir":"C:/Users/fradav/Documents/Dev/Sites/echotone-clean/echotone-web/.vinxi/build/ssr","order":1},{"name":"client","type":"client","base":"/_build","handler":"src/entry-client.tsx","extensions":["js","jsx","ts","tsx"],"target":"browser","root":"C:\\Users\\fradav\\Documents\\Dev\\Sites\\echotone-clean\\echotone-web","outDir":"C:/Users/fradav/Documents/Dev/Sites/echotone-clean/echotone-web/.vinxi/build/client","order":2},{"name":"server-fns","type":"http","base":"/_server","handler":"node_modules/@solidjs/start/dist/runtime/server-handler.js","target":"server","root":"C:\\Users\\fradav\\Documents\\Dev\\Sites\\echotone-clean\\echotone-web","outDir":"C:/Users/fradav/Documents/Dev/Sites/echotone-clean/echotone-web/.vinxi/build/server-fns","order":3}],"server":{"compressPublicAssets":{"brotli":true},"routeRules":{"/_build/assets/**":{"headers":{"cache-control":"public, immutable, max-age=31536000"}}},"experimental":{"asyncContext":true},"preset":"static","output":{"dir":".","publicDir":"www","serverDir":""},"prerender":{}},"root":"C:\\Users\\fradav\\Documents\\Dev\\Sites\\echotone-clean\\echotone-web"};
				const buildManifest = {"ssr":{"virtual:$vinxi/handler/ssr":{"file":"ssr.js","name":"ssr","src":"virtual:$vinxi/handler/ssr","isEntry":true}},"client":{"_HttpStatusCode-DjTx85av.js":{"file":"assets/HttpStatusCode-DjTx85av.js","name":"HttpStatusCode"},"_index-BNyokGjM.js":{"file":"assets/index-BNyokGjM.js","name":"index"},"src/routes/[...404].tsx?pick=default&pick=$css":{"file":"assets/_...404_-4XHyl-XV.js","name":"_...404_","src":"src/routes/[...404].tsx?pick=default&pick=$css","isEntry":true,"isDynamicEntry":true,"imports":["_index-BNyokGjM.js","_HttpStatusCode-DjTx85av.js"]},"src/routes/about.tsx?pick=default&pick=$css":{"file":"assets/about-DBdNjS8Z.js","name":"about","src":"src/routes/about.tsx?pick=default&pick=$css","isEntry":true,"isDynamicEntry":true,"imports":["_index-BNyokGjM.js"]},"src/routes/index.tsx?pick=default&pick=$css":{"file":"assets/index-CiRTup_a.js","name":"index","src":"src/routes/index.tsx?pick=default&pick=$css","isEntry":true,"isDynamicEntry":true,"imports":["_index-BNyokGjM.js"],"css":["assets/index-CWIoMshG.css"]},"virtual:$vinxi/handler/client":{"file":"assets/client-BZXsn40n.js","name":"client","src":"virtual:$vinxi/handler/client","isEntry":true,"imports":["_index-BNyokGjM.js","_HttpStatusCode-DjTx85av.js"],"dynamicImports":["src/routes/about.tsx?pick=default&pick=$css","src/routes/index.tsx?pick=default&pick=$css","src/routes/[...404].tsx?pick=default&pick=$css"],"css":["assets/client-rN2XJZw8.css"]}},"server-fns":{"_server-fns.mjs":{"file":"server-fns.mjs","name":"server-fns","dynamicImports":["src/app.tsx"]},"src/app.tsx":{"file":"app.mjs","name":"app","src":"src/app.tsx","isDynamicEntry":true,"imports":["_server-fns.mjs"],"css":["assets/app-rN2XJZw8.css"]},"virtual:$vinxi/handler/server-fns":{"file":"entry.mjs","name":"entry","src":"virtual:$vinxi/handler/server-fns","isEntry":true,"imports":["_server-fns.mjs"]}}};

				const routeManifest = {"ssr":{},"client":{},"server-fns":{}};

        function createProdApp(appConfig) {
          return {
            config: { ...appConfig, buildManifest, routeManifest },
            getRouter(name) {
              return appConfig.routers.find(router => router.name === name)
            }
          }
        }

        function plugin(app) {
          const prodApp = createProdApp(appConfig$1);
          globalThis.app = prodApp;
        }

const chunks = {};
			 



			 function app() {
				 globalThis.$$chunks = chunks;
			 }

const plugins = [
  plugin,
_olRoFNXOxi,
_riM5SrWQFq,
app
];

const assets$1 = {
  "/favicon.ico": {
    "type": "image/vnd.microsoft.icon",
    "etag": "\"298-hdW7/pL89QptiszdYCHH67XxLxs\"",
    "mtime": "2024-10-31T23:04:09.000Z",
    "size": 664,
    "path": "../www/favicon.ico"
  },
  "/_build/server-functions-manifest.json": {
    "type": "application/json",
    "etag": "\"19-U+evudgPW1yE9kGumdxd/vtvk2s\"",
    "mtime": "2024-11-09T16:16:36.380Z",
    "size": 25,
    "path": "../www/_build/server-functions-manifest.json"
  },
  "/_build/.vite/manifest.json": {
    "type": "application/json",
    "etag": "\"668-ffFIpT2drmOgEb3YcSjVYUDwjEg\"",
    "mtime": "2024-11-09T16:16:36.380Z",
    "size": 1640,
    "path": "../www/_build/.vite/manifest.json"
  },
  "/_build/.vite/manifest.json.br": {
    "type": "application/json",
    "encoding": "br",
    "etag": "\"157-Bu3+wj7XnEPGBhiOYTwu14YXqXg\"",
    "mtime": "2024-11-09T16:16:41.249Z",
    "size": 343,
    "path": "../www/_build/.vite/manifest.json.br"
  },
  "/_build/.vite/manifest.json.gz": {
    "type": "application/json",
    "encoding": "gzip",
    "etag": "\"189-vHcwwbXFTT0AutIIINI7ha2UDbw\"",
    "mtime": "2024-11-09T16:16:41.243Z",
    "size": 393,
    "path": "../www/_build/.vite/manifest.json.gz"
  },
  "/_build/assets/about-DBdNjS8Z.js": {
    "type": "text/javascript; charset=utf-8",
    "etag": "\"cc-8dzqfmAWshTjqFUg2i8pl+hdm5M\"",
    "mtime": "2024-11-09T16:16:36.380Z",
    "size": 204,
    "path": "../www/_build/assets/about-DBdNjS8Z.js"
  },
  "/_build/assets/client-BZXsn40n.js": {
    "type": "text/javascript; charset=utf-8",
    "etag": "\"5586-SPijg+cbjm1gH7EzxBnzkl2IDT0\"",
    "mtime": "2024-11-09T16:16:36.385Z",
    "size": 21894,
    "path": "../www/_build/assets/client-BZXsn40n.js"
  },
  "/_build/assets/client-BZXsn40n.js.br": {
    "type": "text/javascript; charset=utf-8",
    "encoding": "br",
    "etag": "\"1f3c-YPy5fk3MvHHiqGXSV9VvZiq54To\"",
    "mtime": "2024-11-09T16:16:41.285Z",
    "size": 7996,
    "path": "../www/_build/assets/client-BZXsn40n.js.br"
  },
  "/_build/assets/client-BZXsn40n.js.gz": {
    "type": "text/javascript; charset=utf-8",
    "encoding": "gzip",
    "etag": "\"2295-tbkga0HjfiD0s3q2NuPdXC52UMg\"",
    "mtime": "2024-11-09T16:16:41.243Z",
    "size": 8853,
    "path": "../www/_build/assets/client-BZXsn40n.js.gz"
  },
  "/_build/assets/client-rN2XJZw8.css": {
    "type": "text/css; charset=utf-8",
    "etag": "\"17f-EyPT40t0Q2Ip557OUJ0k5vttT/g\"",
    "mtime": "2024-11-09T16:16:36.384Z",
    "size": 383,
    "path": "../www/_build/assets/client-rN2XJZw8.css"
  },
  "/_build/assets/HttpStatusCode-DjTx85av.js": {
    "type": "text/javascript; charset=utf-8",
    "etag": "\"20-6m70mxigcQrfQOHf/Wz+MEC183U\"",
    "mtime": "2024-11-09T16:16:36.380Z",
    "size": 32,
    "path": "../www/_build/assets/HttpStatusCode-DjTx85av.js"
  },
  "/_build/assets/index-BNyokGjM.js": {
    "type": "text/javascript; charset=utf-8",
    "etag": "\"5b84-kUntgB4MTB234XT/TV6i0Xyj/lI\"",
    "mtime": "2024-11-09T16:16:36.385Z",
    "size": 23428,
    "path": "../www/_build/assets/index-BNyokGjM.js"
  },
  "/_build/assets/index-BNyokGjM.js.br": {
    "type": "text/javascript; charset=utf-8",
    "encoding": "br",
    "etag": "\"205d-W3Hshiz0J2VH2KW1wQBc8w2URIQ\"",
    "mtime": "2024-11-09T16:16:41.297Z",
    "size": 8285,
    "path": "../www/_build/assets/index-BNyokGjM.js.br"
  },
  "/_build/assets/index-BNyokGjM.js.gz": {
    "type": "text/javascript; charset=utf-8",
    "encoding": "gzip",
    "etag": "\"2393-KxDhMUxC2/JRL4jbbEKVwo8N44A\"",
    "mtime": "2024-11-09T16:16:41.243Z",
    "size": 9107,
    "path": "../www/_build/assets/index-BNyokGjM.js.gz"
  },
  "/_build/assets/index-CiRTup_a.js": {
    "type": "text/javascript; charset=utf-8",
    "etag": "\"235-vHvS4h+XrZKjtBDMtf9lOdu4TAE\"",
    "mtime": "2024-11-09T16:16:36.386Z",
    "size": 565,
    "path": "../www/_build/assets/index-CiRTup_a.js"
  },
  "/_build/assets/index-CWIoMshG.css": {
    "type": "text/css; charset=utf-8",
    "etag": "\"142-YFC3EOCQaL0Cbn8eYT95QLqQW+M\"",
    "mtime": "2024-11-09T16:16:36.385Z",
    "size": 322,
    "path": "../www/_build/assets/index-CWIoMshG.css"
  },
  "/_build/assets/_...404_-4XHyl-XV.js": {
    "type": "text/javascript; charset=utf-8",
    "etag": "\"19b-AlQPyL9mwG2QBL4npA9llD2TgZw\"",
    "mtime": "2024-11-09T16:16:36.386Z",
    "size": 411,
    "path": "../www/_build/assets/_...404_-4XHyl-XV.js"
  },
  "/_server/assets/app-rN2XJZw8.css": {
    "type": "text/css; charset=utf-8",
    "etag": "\"17f-EyPT40t0Q2Ip557OUJ0k5vttT/g\"",
    "mtime": "2024-11-09T16:16:40.605Z",
    "size": 383,
    "path": "../www/_server/assets/app-rN2XJZw8.css"
  }
};

function readAsset (id) {
  const serverDir = dirname(fileURLToPath(globalThis._importMeta_.url));
  return promises.readFile(resolve(serverDir, assets$1[id].path))
}

const publicAssetBases = {};

function isPublicAssetURL(id = '') {
  if (assets$1[id]) {
    return true
  }
  for (const base in publicAssetBases) {
    if (id.startsWith(base)) { return true }
  }
  return false
}

function getAsset (id) {
  return assets$1[id]
}

const METHODS = /* @__PURE__ */ new Set(["HEAD", "GET"]);
const EncodingMap = { gzip: ".gz", br: ".br" };
const _TqCTWi = eventHandler((event) => {
  if (event.method && !METHODS.has(event.method)) {
    return;
  }
  let id = decodePath(
    withLeadingSlash(withoutTrailingSlash(parseURL(event.path).pathname))
  );
  let asset;
  const encodingHeader = String(
    getRequestHeader(event, "accept-encoding") || ""
  );
  const encodings = [
    ...encodingHeader.split(",").map((e) => EncodingMap[e.trim()]).filter(Boolean).sort(),
    ""
  ];
  if (encodings.length > 1) {
    appendResponseHeader(event, "Vary", "Accept-Encoding");
  }
  for (const encoding of encodings) {
    for (const _id of [id + encoding, joinURL(id, "index.html" + encoding)]) {
      const _asset = getAsset(_id);
      if (_asset) {
        asset = _asset;
        id = _id;
        break;
      }
    }
  }
  if (!asset) {
    if (isPublicAssetURL(id)) {
      removeResponseHeader(event, "Cache-Control");
      throw createError({
        statusMessage: "Cannot find static asset " + id,
        statusCode: 404
      });
    }
    return;
  }
  const ifNotMatch = getRequestHeader(event, "if-none-match") === asset.etag;
  if (ifNotMatch) {
    setResponseStatus(event, 304, "Not Modified");
    return "";
  }
  const ifModifiedSinceH = getRequestHeader(event, "if-modified-since");
  const mtimeDate = new Date(asset.mtime);
  if (ifModifiedSinceH && asset.mtime && new Date(ifModifiedSinceH) >= mtimeDate) {
    setResponseStatus(event, 304, "Not Modified");
    return "";
  }
  if (asset.type && !getResponseHeader(event, "Content-Type")) {
    setResponseHeader(event, "Content-Type", asset.type);
  }
  if (asset.etag && !getResponseHeader(event, "ETag")) {
    setResponseHeader(event, "ETag", asset.etag);
  }
  if (asset.mtime && !getResponseHeader(event, "Last-Modified")) {
    setResponseHeader(event, "Last-Modified", mtimeDate.toUTCString());
  }
  if (asset.encoding && !getResponseHeader(event, "Content-Encoding")) {
    setResponseHeader(event, "Content-Encoding", asset.encoding);
  }
  if (asset.size > 0 && !getResponseHeader(event, "Content-Length")) {
    setResponseHeader(event, "Content-Length", asset.size);
  }
  return readAsset(id);
});

function Ee$1(e){let n;const t=G(e),r={duplex:"half",method:e.method,headers:e.headers};return e.node.req.body instanceof ArrayBuffer?new Request(t,{...r,body:e.node.req.body}):new Request(t,{...r,get body(){return n||(n=Ue$1(e),n)}})}function xe$1(e){return e.web??={request:Ee$1(e),url:G(e)},e.web.request}function He$1(){return je$1()}const J=Symbol("$HTTPEvent");function Pe$1(e){return typeof e=="object"&&(e instanceof H3Event||e?.[J]instanceof H3Event||e?.__is_event__===!0)}function u(e){return function(...n){let t=n[0];if(Pe$1(t))n[0]=t instanceof H3Event||t.__is_event__?t:t[J];else {if(!globalThis.app.config.server.experimental?.asyncContext)throw new Error("AsyncLocalStorage was not enabled. Use the `server.experimental.asyncContext: true` option in your app configuration to enable it. Or, pass the instance of HTTPEvent that you have as the first argument to the function.");if(t=He$1(),!t)throw new Error("No HTTPEvent found in AsyncLocalStorage. Make sure you are using the function within the server runtime.");n.unshift(t);}return e(...n)}}const G=u(getRequestURL),Ce$1=u(getRequestIP),R=u(setResponseStatus),_=u(getResponseStatus),Ae=u(getResponseStatusText),m=u(getResponseHeaders),j=u(getResponseHeader),ke$1=u(setResponseHeader),K=u(appendResponseHeader),Le$1=u(getCookie),Fe$1=u(setCookie),y=u(setHeader),Ue$1=u(getRequestWebStream),Oe$1=u(removeResponseHeader),Ie$1=u(xe$1);function _e$1(){return getContext("nitro-app",{asyncContext:!!globalThis.app.config.server.experimental?.asyncContext,AsyncLocalStorage:AsyncLocalStorage})}function je$1(){return _e$1().use().event}const S$1="Invariant Violation",{setPrototypeOf:De$1=function(e,n){return e.__proto__=n,e}}=Object;class L extends Error{framesToPop=1;name=S$1;constructor(n=S$1){super(typeof n=="number"?`${S$1}: ${n} (see https://github.com/apollographql/invariant-packages)`:n),De$1(this,L.prototype);}}function Me$1(e,n){if(!e)throw new L(n)}const w="solidFetchEvent";function We$1(e){return {request:Ie$1(e),response:ze(e),clientAddress:Ce$1(e),locals:{},nativeEvent:e}}function Ne$1(e){return {...e}}function Be$1(e){if(!e.context[w]){const n=We$1(e);e.context[w]=n;}return e.context[w]}function D(e,n){for(const[t,r]of n.entries())K(e,t,r);}class Xe{event;constructor(n){this.event=n;}get(n){const t=j(this.event,n);return Array.isArray(t)?t.join(", "):t||null}has(n){return this.get(n)!==void 0}set(n,t){return ke$1(this.event,n,t)}delete(n){return Oe$1(this.event,n)}append(n,t){K(this.event,n,t);}getSetCookie(){const n=j(this.event,"Set-Cookie");return Array.isArray(n)?n:[n]}forEach(n){return Object.entries(m(this.event)).forEach(([t,r])=>n(Array.isArray(r)?r.join(", "):r,t,this))}entries(){return Object.entries(m(this.event)).map(([n,t])=>[n,Array.isArray(t)?t.join(", "):t])[Symbol.iterator]()}keys(){return Object.keys(m(this.event))[Symbol.iterator]()}values(){return Object.values(m(this.event)).map(n=>Array.isArray(n)?n.join(", "):n)[Symbol.iterator]()}[Symbol.iterator](){return this.entries()[Symbol.iterator]()}}function ze(e){return {get status(){return _(e)},set status(n){R(e,n);},get statusText(){return Ae(e)},set statusText(n){R(e,_(e),n);},headers:new Xe(e)}}const V=[{page:!0,path:"/about",filePath:"C:/Users/fradav/Documents/Dev/Sites/echotone-clean/echotone-web/src/routes/about.tsx"},{page:!0,path:"/",filePath:"C:/Users/fradav/Documents/Dev/Sites/echotone-clean/echotone-web/src/routes/index.tsx"},{page:!0,path:"/*404",filePath:"C:/Users/fradav/Documents/Dev/Sites/echotone-clean/echotone-web/src/routes/[...404].tsx"}],Je=Ge$1(V.filter(e=>e.page));function Ge$1(e){function n(t,r,o,i){const c=Object.values(t).find(a=>o.startsWith(a.id+"/"));return c?(n(c.children||(c.children=[]),r,o.slice(c.id.length)),t):(t.push({...r,id:o,path:o.replace(/\/\([^)/]+\)/g,"").replace(/\([^)/]+\)/g,"")}),t)}return e.sort((t,r)=>t.path.length-r.path.length).reduce((t,r)=>n(t,r,r.path,r.path),[])}function Ke(e){return e.$HEAD||e.$GET||e.$POST||e.$PUT||e.$PATCH||e.$DELETE}createRouter({routes:V.reduce((e,n)=>{if(!Ke(n))return e;let t=n.path.replace(/\/\([^)/]+\)/g,"").replace(/\([^)/]+\)/g,"").replace(/\*([^/]*)/g,(r,o)=>`**:${o}`).split("/").map(r=>r.startsWith(":")||r.startsWith("*")?r:encodeURIComponent(r)).join("/");if(/:[^/]*\?/g.test(t))throw new Error(`Optional parameters are not supported in API routes: ${t}`);if(e[t])throw new Error(`Duplicate API routes for "${t}" found at "${e[t].route.path}" and "${n.path}"`);return e[t]={route:n},e},{})});var Qe=" ";const Ye={style:e=>ssrElement("style",e.attrs,()=>escape(e.children),!0),link:e=>ssrElement("link",e.attrs,void 0,!0),script:e=>e.attrs.src?ssrElement("script",mergeProps(()=>e.attrs,{get id(){return e.key}}),()=>ssr(Qe),!0):null,noscript:e=>ssrElement("noscript",e.attrs,()=>escape(e.children),!0)};function Ze$1(e,n){let{tag:t,attrs:{key:r,...o}={key:void 0},children:i}=e;return Ye[t]({attrs:{...o,nonce:n},key:r,children:i})}function et(e,n,t,r="default"){return lazy(async()=>{{const i=(await e.import())[r],a=(await n.inputs?.[e.src].assets()).filter(p=>p.tag==="style"||p.attrs.rel==="stylesheet");return {default:p=>[...a.map(f=>Ze$1(f)),createComponent(i,p)]}}})}function Q(){function e(t){return {...t,...t.$$route?t.$$route.require().route:void 0,info:{...t.$$route?t.$$route.require().route.info:{},filesystem:!0},component:t.$component&&et(t.$component,globalThis.MANIFEST.client,globalThis.MANIFEST.ssr),children:t.children?t.children.map(e):void 0}}return Je.map(e)}let M;const mt=isServer?()=>getRequestEvent().routes:()=>M||(M=Q());function tt(e){const n=Le$1(e.nativeEvent,"flash");if(n)try{let t=JSON.parse(n);if(!t||!t.result)return;const r=[...t.input.slice(0,-1),new Map(t.input[t.input.length-1])],o=t.error?new Error(t.result):t.result;return {input:r,url:t.url,pending:!1,result:t.thrown?void 0:o,error:t.thrown?o:void 0}}catch(t){console.error(t);}finally{Fe$1(e.nativeEvent,"flash","",{maxAge:0});}}async function nt(e){const n=globalThis.MANIFEST.client;return globalThis.MANIFEST.ssr,e.response.headers.set("Content-Type","text/html"),Object.assign(e,{manifest:await n.json(),assets:[...await n.inputs[n.handler].assets()],router:{submission:tt(e)},routes:Q(),complete:!1,$islands:new Set})}const st=new Set([301,302,303,307,308]);function rt(e){return e.status&&st.has(e.status)?e.status:302}function ot(e){const n=new TextEncoder().encode(e),t=n.length,r=t.toString(16),o="00000000".substring(0,8-r.length)+r,i=new TextEncoder().encode(`;0x${o};`),c=new Uint8Array(12+t);return c.set(i),c.set(n,12),c}function W(e,n){return new ReadableStream({start(t){crossSerializeStream(n,{scopeId:e,plugins:[CustomEventPlugin,DOMExceptionPlugin,EventPlugin,FormDataPlugin,HeadersPlugin,ReadableStreamPlugin,RequestPlugin,ResponsePlugin,URLSearchParamsPlugin,URLPlugin],onSerialize(r,o){t.enqueue(ot(o?`(${getCrossReferenceHeader(e)},${r})`:r));},onDone(){t.close();},onError(r){t.error(r);}});}})}async function at(e){const n=Be$1(e),t=n.request,r=t.headers.get("X-Server-Id"),o=t.headers.get("X-Server-Instance"),i=t.headers.has("X-Single-Flight"),c=new URL(t.url);let a,d;if(r)Me$1(typeof r=="string","Invalid server function"),[a,d]=r.split("#");else if(a=c.searchParams.get("id"),d=c.searchParams.get("name"),!a||!d)throw new Error("Invalid request");const p=(await globalThis.MANIFEST["server-fns"].chunks[a].import())[d];let f=[];if(!o||e.method==="GET"){const s=c.searchParams.get("args");if(s){const l=JSON.parse(s);(l.t?fromJSON(l,{plugins:[CustomEventPlugin,DOMExceptionPlugin,EventPlugin,FormDataPlugin,HeadersPlugin,ReadableStreamPlugin,RequestPlugin,ResponsePlugin,URLSearchParamsPlugin,URLPlugin]}):l).forEach(h=>f.push(h));}}if(e.method==="POST"){const s=t.headers.get("content-type"),l=e.node.req,h=l instanceof ReadableStream,F=h&&l.locked,U=h?l:l.body;if(s?.startsWith("multipart/form-data")||s?.startsWith("application/x-www-form-urlencoded"))f.push(await(F?t:new Request(t,{...t,body:U})).formData());else if(s?.startsWith("application/json")){const Y=F?t:new Request(t,{...t,body:U});f=fromJSON(await Y.json(),{plugins:[CustomEventPlugin,DOMExceptionPlugin,EventPlugin,FormDataPlugin,HeadersPlugin,ReadableStreamPlugin,RequestPlugin,ResponsePlugin,URLSearchParamsPlugin,URLPlugin]});}}try{let s=await provideRequestEvent(n,async()=>(sharedConfig.context={event:n},n.locals.serverFunctionMeta={id:a+"#"+d},p(...f)));if(i&&o&&(s=await B(n,s)),s instanceof Response){if(s.headers&&s.headers.has("X-Content-Raw"))return s;o&&(s.headers&&D(e,s.headers),s.status&&(s.status<300||s.status>=400)&&R(e,s.status),s.customBody?s=await s.customBody():s.body==null&&(s=null));}return o?(y(e,"content-type","text/javascript"),W(o,s)):N(s,t,f)}catch(s){if(s instanceof Response)i&&o&&(s=await B(n,s)),s.headers&&D(e,s.headers),s.status&&(!o||s.status<300||s.status>=400)&&R(e,s.status),s.customBody?s=s.customBody():s.body==null&&(s=null),y(e,"X-Error","true");else if(o){const l=s instanceof Error?s.message:typeof s=="string"?s:"true";y(e,"X-Error",l.replace(/[\r\n]+/g,""));}else s=N(s,t,f,!0);return o?(y(e,"content-type","text/javascript"),W(o,s)):s}}function N(e,n,t,r){const o=new URL(n.url),i=e instanceof Error;let c=302,a;return e instanceof Response?(a=new Headers(e.headers),e.headers.has("Location")&&(a.set("Location",new URL(e.headers.get("Location"),o.origin+"").toString()),c=rt(e))):a=new Headers({Location:new URL(n.headers.get("referer")).toString()}),e&&a.append("Set-Cookie",`flash=${encodeURIComponent(JSON.stringify({url:o.pathname+o.search,result:i?e.message:e,thrown:r,error:i,input:[...t.slice(0,-1),[...t[t.length-1].entries()]]}))}; Secure; HttpOnly;`),new Response(null,{status:c,headers:a})}let v;async function B(e,n){let t,r=new URL(e.request.headers.get("referer")).toString();n instanceof Response&&(n.headers.has("X-Revalidate")&&(t=n.headers.get("X-Revalidate").split(",")),n.headers.has("Location")&&(r=new URL(n.headers.get("Location"),new URL(e.request.url).origin+"").toString()));const o=Ne$1(e);return o.request=new Request(r,{headers:e.request.headers}),await provideRequestEvent(o,async()=>{await nt(o),v||(v=(await import('../build/app.mjs')).default),o.router.dataOnly=t||!0,o.router.previousUrl=e.request.headers.get("referer");try{renderToString(()=>{sharedConfig.context.event=o,v();});}catch(a){console.log(a);}const i=o.router.data;if(!i)return n;let c=!1;for(const a in i)i[a]===void 0?delete i[a]:c=!0;return c&&(n instanceof Response?n.customBody&&(i._$value=n.customBody()):(i._$value=n,n=new Response(null,{status:200})),n.customBody=()=>i,n.headers.set("X-Single-Flight","true")),n})}const yt=eventHandler(at);

var __defProp = Object.defineProperty;
var __defNormalProp = (obj, key, value) => key in obj ? __defProp(obj, key, { enumerable: true, configurable: true, writable: true, value }) : obj[key] = value;
var __publicField = (obj, key, value) => __defNormalProp(obj, key + "" , value);
const le = isServer ? (e) => {
  const t = getRequestEvent();
  return t.response.status = e.code, t.response.statusText = e.text, onCleanup(() => !t.nativeEvent.handled && !t.complete && (t.response.status = 200)), null;
} : (e) => null;
var pe = ["<span", ' style="font-size:1.5em;text-align:center;position:fixed;left:0px;bottom:55%;width:100%;">500 | Internal Server Error</span>'];
const de = (e) => {
  let t = false;
  const n = catchError(() => e.children, (r) => {
    console.error(r), t = !!r;
  });
  return t ? [ssr(pe, ssrHydrationKey()), createComponent$1(le, { code: 500 })] : n;
};
var he = " ";
const fe = { style: (e) => ssrElement("style", e.attrs, () => escape(e.children), true), link: (e) => ssrElement("link", e.attrs, void 0, true), script: (e) => e.attrs.src ? ssrElement("script", mergeProps(() => e.attrs, { get id() {
  return e.key;
} }), () => ssr(he), true) : null, noscript: (e) => ssrElement("noscript", e.attrs, () => escape(e.children), true) };
function me(e, t) {
  let { tag: n, attrs: { key: r, ...o } = { key: void 0 }, children: d } = e;
  return fe[n]({ attrs: { ...o, nonce: t }, key: r, children: d });
}
var A = ["<script", ">", "<\/script>"], H = ["<script", ' type="module"', "><\/script>"];
const ge = ssr("<!DOCTYPE html>");
function ye(e) {
  const t = getRequestEvent(), n = t.nonce;
  return createComponent$1(NoHydration, { get children() {
    return [ge, createComponent$1(de, { get children() {
      return createComponent$1(e.document, { get assets() {
        return t.assets.map((r) => me(r));
      }, get scripts() {
        return n ? [ssr(A, ssrHydrationKey() + ssrAttribute("nonce", escape(n, true), false), `window.manifest = ${JSON.stringify(t.manifest)}`), ssr(H, ssrHydrationKey(), ssrAttribute("src", escape(globalThis.MANIFEST.client.inputs[globalThis.MANIFEST.client.handler].output.path, true), false))] : [ssr(A, ssrHydrationKey(), `window.manifest = ${JSON.stringify(t.manifest)}`), ssr(H, ssrHydrationKey(), ssrAttribute("src", escape(globalThis.MANIFEST.client.inputs[globalThis.MANIFEST.client.handler].output.path, true), false))];
      } });
    } })];
  } });
}
function Re(e) {
  let t;
  const n = k(e), r = { duplex: "half", method: e.method, headers: e.headers };
  return e.node.req.body instanceof ArrayBuffer ? new Request(n, { ...r, body: e.node.req.body }) : new Request(n, { ...r, get body() {
    return t || (t = He(e), t);
  } });
}
function Se(e) {
  var _a;
  return (_a = e.web) != null ? _a : e.web = { request: Re(e), url: k(e) }, e.web.request;
}
function ve() {
  return Pe();
}
const I = Symbol("$HTTPEvent");
function Ee(e) {
  return typeof e == "object" && (e instanceof H3Event || (e == null ? void 0 : e[I]) instanceof H3Event || (e == null ? void 0 : e.__is_event__) === true);
}
function i(e) {
  return function(...t) {
    var _a;
    let n = t[0];
    if (Ee(n)) t[0] = n instanceof H3Event || n.__is_event__ ? n : n[I];
    else {
      if (!((_a = globalThis.app.config.server.experimental) == null ? void 0 : _a.asyncContext)) throw new Error("AsyncLocalStorage was not enabled. Use the `server.experimental.asyncContext: true` option in your app configuration to enable it. Or, pass the instance of HTTPEvent that you have as the first argument to the function.");
      if (n = ve(), !n) throw new Error("No HTTPEvent found in AsyncLocalStorage. Make sure you are using the function within the server runtime.");
      t.unshift(n);
    }
    return e(...t);
  };
}
const k = i(getRequestURL), be = i(getRequestIP), E = i(setResponseStatus), x = i(getResponseStatus), $e = i(getResponseStatusText), g = i(getResponseHeaders), C = i(getResponseHeader), Te = i(setResponseHeader), we = i(appendResponseHeader), q = i(sendRedirect), He = i(getRequestWebStream), xe = i(removeResponseHeader), Ce = i(Se);
function qe() {
  var _a;
  return getContext("nitro-app", { asyncContext: !!((_a = globalThis.app.config.server.experimental) == null ? void 0 : _a.asyncContext), AsyncLocalStorage: AsyncLocalStorage });
}
function Pe() {
  return qe().use().event;
}
const O = [{ page: true, path: "/about", filePath: "C:/Users/fradav/Documents/Dev/Sites/echotone-clean/echotone-web/src/routes/about.tsx" }, { page: true, path: "/", filePath: "C:/Users/fradav/Documents/Dev/Sites/echotone-clean/echotone-web/src/routes/index.tsx" }, { page: true, path: "/*404", filePath: "C:/Users/fradav/Documents/Dev/Sites/echotone-clean/echotone-web/src/routes/[...404].tsx" }];
_e(O.filter((e) => e.page));
function _e(e) {
  function t(n, r, o, d) {
    const s = Object.values(n).find((a) => o.startsWith(a.id + "/"));
    return s ? (t(s.children || (s.children = []), r, o.slice(s.id.length)), n) : (n.push({ ...r, id: o, path: o.replace(/\/\([^)/]+\)/g, "").replace(/\([^)/]+\)/g, "") }), n);
  }
  return e.sort((n, r) => n.path.length - r.path.length).reduce((n, r) => t(n, r, r.path, r.path), []);
}
function Le(e, t) {
  const n = Ie.lookup(e);
  if (n && n.route) {
    const r = t === "HEAD" ? n.route.$HEAD || n.route.$GET : n.route[`$${t}`];
    return r === void 0 ? void 0 : { handler: r, params: n.params };
  }
}
function De(e) {
  return e.$HEAD || e.$GET || e.$POST || e.$PUT || e.$PATCH || e.$DELETE;
}
const Ie = createRouter({ routes: O.reduce((e, t) => {
  if (!De(t)) return e;
  let n = t.path.replace(/\/\([^)/]+\)/g, "").replace(/\([^)/]+\)/g, "").replace(/\*([^/]*)/g, (r, o) => `**:${o}`).split("/").map((r) => r.startsWith(":") || r.startsWith("*") ? r : encodeURIComponent(r)).join("/");
  if (/:[^/]*\?/g.test(n)) throw new Error(`Optional parameters are not supported in API routes: ${n}`);
  if (e[n]) throw new Error(`Duplicate API routes for "${n}" found at "${e[n].route.path}" and "${t.path}"`);
  return e[n] = { route: t }, e;
}, {}) }), S = "solidFetchEvent";
function ke(e) {
  return { request: Ce(e), response: Ne(e), clientAddress: be(e), locals: {}, nativeEvent: e };
}
function Oe(e) {
  if (!e.context[S]) {
    const t = ke(e);
    e.context[S] = t;
  }
  return e.context[S];
}
class je {
  constructor(t) {
    __publicField(this, "event");
    this.event = t;
  }
  get(t) {
    const n = C(this.event, t);
    return Array.isArray(n) ? n.join(", ") : n || null;
  }
  has(t) {
    return this.get(t) !== void 0;
  }
  set(t, n) {
    return Te(this.event, t, n);
  }
  delete(t) {
    return xe(this.event, t);
  }
  append(t, n) {
    we(this.event, t, n);
  }
  getSetCookie() {
    const t = C(this.event, "Set-Cookie");
    return Array.isArray(t) ? t : [t];
  }
  forEach(t) {
    return Object.entries(g(this.event)).forEach(([n, r]) => t(Array.isArray(r) ? r.join(", ") : r, n, this));
  }
  entries() {
    return Object.entries(g(this.event)).map(([t, n]) => [t, Array.isArray(n) ? n.join(", ") : n])[Symbol.iterator]();
  }
  keys() {
    return Object.keys(g(this.event))[Symbol.iterator]();
  }
  values() {
    return Object.values(g(this.event)).map((t) => Array.isArray(t) ? t.join(", ") : t)[Symbol.iterator]();
  }
  [Symbol.iterator]() {
    return this.entries()[Symbol.iterator]();
  }
}
function Ne(e) {
  return { get status() {
    return x(e);
  }, set status(t) {
    E(e, t);
  }, get statusText() {
    return $e(e);
  }, set statusText(t) {
    E(e, x(e), t);
  }, headers: new je(e) };
}
const Ue = /* @__PURE__ */ new Set([301, 302, 303, 307, 308]);
function b(e) {
  return e.status && Ue.has(e.status) ? e.status : 302;
}
function We(e, t, n = {}) {
  return eventHandler({ handler: (r) => {
    const o = Oe(r);
    return provideRequestEvent(o, async () => {
      const d = Le(new URL(o.request.url).pathname, o.request.method);
      if (d) {
        const c = await d.handler.import(), l = o.request.method === "HEAD" ? c.HEAD || c.GET : c[o.request.method];
        o.params = d.params || {}, sharedConfig.context = { event: o };
        const w = await l(o);
        if (w !== void 0) return w;
        if (o.request.method !== "GET") throw new Error(`API handler for ${o.request.method} "${o.request.url}" did not return a response.`);
      }
      const s = await t(o), a = typeof n == "function" ? await n(s) : { ...n }; a.mode || "stream";
      a.nonce && (s.nonce = a.nonce);
      {
        const c = renderToString(() => (sharedConfig.context.event = s, e(s)), a);
        if (s.complete = true, s.response && s.response.headers.get("Location")) {
          const l = b(s.response);
          return q(r, s.response.headers.get("Location"), l);
        }
        return c;
      }
    });
  } });
}
function Fe(e, t) {
  return We(e, Me, t);
}
async function Me(e) {
  const t = globalThis.MANIFEST.client;
  return Object.assign(e, { manifest: await t.json(), assets: [...await t.inputs[t.handler].assets()], routes: [], complete: false, $islands: /* @__PURE__ */ new Set() });
}
var Ge = ['<head><meta charset="utf-8"><meta name="viewport" content="width=device-width, initial-scale=1"><link rel="icon" href="/favicon.ico">', "</head>"], Be = ["<html", ' lang="en">', '<body><div id="app">', "</div><!--$-->", "<!--/--></body></html>"];
const Ze = Fe(() => createComponent$1(ye, { document: ({ assets: e, children: t, scripts: n }) => ssr(Be, ssrHydrationKey(), createComponent$1(NoHydration, { get children() {
  return ssr(Ge, escape(e));
} }), escape(t), escape(n)) }));

const handlers = [
  { route: '', handler: _TqCTWi, lazy: false, middleware: true, method: undefined },
  { route: '/_server', handler: yt, lazy: false, middleware: true, method: undefined },
  { route: '/', handler: Ze, lazy: false, middleware: true, method: undefined }
];

const serverAssets = [{"baseName":"server","dir":"C:/Users/fradav/Documents/Dev/Sites/echotone-clean/echotone-web/assets"}];

const assets = createStorage();

for (const asset of serverAssets) {
  assets.mount(asset.baseName, unstorage_47drivers_47fs({ base: asset.dir, ignore: (asset?.ignore || []) }));
}

const storage = createStorage({});

storage.mount('/assets', assets);

storage.mount('data', unstorage_47drivers_47fs_45lite({"driver":"fsLite","base":"C:\\Users\\fradav\\Documents\\Dev\\Sites\\echotone-clean\\echotone-web\\.data\\kv"}));
storage.mount('root', unstorage_47drivers_47fs({"driver":"fs","readOnly":true,"base":"C:\\Users\\fradav\\Documents\\Dev\\Sites\\echotone-clean\\echotone-web","ignore":["**/node_modules/**","**/.git/**"]}));
storage.mount('src', unstorage_47drivers_47fs({"driver":"fs","readOnly":true,"base":"C:\\Users\\fradav\\Documents\\Dev\\Sites\\echotone-clean\\echotone-web","ignore":["**/node_modules/**","**/.git/**"]}));
storage.mount('build', unstorage_47drivers_47fs({"driver":"fs","readOnly":false,"base":"C:\\Users\\fradav\\Documents\\Dev\\Sites\\echotone-clean\\echotone-web\\.vinxi","ignore":["**/node_modules/**","**/.git/**"]}));
storage.mount('cache', unstorage_47drivers_47fs({"driver":"fs","readOnly":false,"base":"C:\\Users\\fradav\\Documents\\Dev\\Sites\\echotone-clean\\echotone-web\\.vinxi\\cache","ignore":["**/node_modules/**","**/.git/**"]}));

function useStorage(base = "") {
  return base ? prefixStorage(storage, base) : storage;
}

function defaultCacheOptions() {
  return {
    name: "_",
    base: "/cache",
    swr: true,
    maxAge: 1
  };
}
function defineCachedFunction(fn, opts = {}) {
  opts = { ...defaultCacheOptions(), ...opts };
  const pending = {};
  const group = opts.group || "nitro/functions";
  const name = opts.name || fn.name || "_";
  const integrity = opts.integrity || hash([fn, opts]);
  const validate = opts.validate || ((entry) => entry.value !== void 0);
  async function get(key, resolver, shouldInvalidateCache, event) {
    const cacheKey = [opts.base, group, name, key + ".json"].filter(Boolean).join(":").replace(/:\/$/, ":index");
    let entry = await useStorage().getItem(cacheKey).catch((error) => {
      console.error(`[nitro] [cache] Cache read error.`, error);
      useNitroApp().captureError(error, { event, tags: ["cache"] });
    }) || {};
    if (typeof entry !== "object") {
      entry = {};
      const error = new Error("Malformed data read from cache.");
      console.error("[nitro] [cache]", error);
      useNitroApp().captureError(error, { event, tags: ["cache"] });
    }
    const ttl = (opts.maxAge ?? 0) * 1e3;
    if (ttl) {
      entry.expires = Date.now() + ttl;
    }
    const expired = shouldInvalidateCache || entry.integrity !== integrity || ttl && Date.now() - (entry.mtime || 0) > ttl || validate(entry) === false;
    const _resolve = async () => {
      const isPending = pending[key];
      if (!isPending) {
        if (entry.value !== void 0 && (opts.staleMaxAge || 0) >= 0 && opts.swr === false) {
          entry.value = void 0;
          entry.integrity = void 0;
          entry.mtime = void 0;
          entry.expires = void 0;
        }
        pending[key] = Promise.resolve(resolver());
      }
      try {
        entry.value = await pending[key];
      } catch (error) {
        if (!isPending) {
          delete pending[key];
        }
        throw error;
      }
      if (!isPending) {
        entry.mtime = Date.now();
        entry.integrity = integrity;
        delete pending[key];
        if (validate(entry) !== false) {
          let setOpts;
          if (opts.maxAge && !opts.swr) {
            setOpts = { ttl: opts.maxAge };
          }
          const promise = useStorage().setItem(cacheKey, entry, setOpts).catch((error) => {
            console.error(`[nitro] [cache] Cache write error.`, error);
            useNitroApp().captureError(error, { event, tags: ["cache"] });
          });
          if (event?.waitUntil) {
            event.waitUntil(promise);
          }
        }
      }
    };
    const _resolvePromise = expired ? _resolve() : Promise.resolve();
    if (entry.value === void 0) {
      await _resolvePromise;
    } else if (expired && event && event.waitUntil) {
      event.waitUntil(_resolvePromise);
    }
    if (opts.swr && validate(entry) !== false) {
      _resolvePromise.catch((error) => {
        console.error(`[nitro] [cache] SWR handler error.`, error);
        useNitroApp().captureError(error, { event, tags: ["cache"] });
      });
      return entry;
    }
    return _resolvePromise.then(() => entry);
  }
  return async (...args) => {
    const shouldBypassCache = await opts.shouldBypassCache?.(...args);
    if (shouldBypassCache) {
      return fn(...args);
    }
    const key = await (opts.getKey || getKey)(...args);
    const shouldInvalidateCache = await opts.shouldInvalidateCache?.(...args);
    const entry = await get(
      key,
      () => fn(...args),
      shouldInvalidateCache,
      args[0] && isEvent(args[0]) ? args[0] : void 0
    );
    let value = entry.value;
    if (opts.transform) {
      value = await opts.transform(entry, ...args) || value;
    }
    return value;
  };
}
function cachedFunction(fn, opts = {}) {
  return defineCachedFunction(fn, opts);
}
function getKey(...args) {
  return args.length > 0 ? hash(args, {}) : "";
}
function escapeKey(key) {
  return String(key).replace(/\W/g, "");
}
function defineCachedEventHandler(handler, opts = defaultCacheOptions()) {
  const variableHeaderNames = (opts.varies || []).filter(Boolean).map((h) => h.toLowerCase()).sort();
  const _opts = {
    ...opts,
    getKey: async (event) => {
      const customKey = await opts.getKey?.(event);
      if (customKey) {
        return escapeKey(customKey);
      }
      const _path = event.node.req.originalUrl || event.node.req.url || event.path;
      let _pathname;
      try {
        _pathname = escapeKey(decodeURI(parseURL(_path).pathname)).slice(0, 16) || "index";
      } catch {
        _pathname = "-";
      }
      const _hashedPath = `${_pathname}.${hash(_path)}`;
      const _headers = variableHeaderNames.map((header) => [header, event.node.req.headers[header]]).map(([name, value]) => `${escapeKey(name)}.${hash(value)}`);
      return [_hashedPath, ..._headers].join(":");
    },
    validate: (entry) => {
      if (!entry.value) {
        return false;
      }
      if (entry.value.code >= 400) {
        return false;
      }
      if (entry.value.body === void 0) {
        return false;
      }
      if (entry.value.headers.etag === "undefined" || entry.value.headers["last-modified"] === "undefined") {
        return false;
      }
      return true;
    },
    group: opts.group || "nitro/handlers",
    integrity: opts.integrity || hash([handler, opts])
  };
  const _cachedHandler = cachedFunction(
    async (incomingEvent) => {
      const variableHeaders = {};
      for (const header of variableHeaderNames) {
        const value = incomingEvent.node.req.headers[header];
        if (value !== void 0) {
          variableHeaders[header] = value;
        }
      }
      const reqProxy = cloneWithProxy(incomingEvent.node.req, {
        headers: variableHeaders
      });
      const resHeaders = {};
      let _resSendBody;
      const resProxy = cloneWithProxy(incomingEvent.node.res, {
        statusCode: 200,
        writableEnded: false,
        writableFinished: false,
        headersSent: false,
        closed: false,
        getHeader(name) {
          return resHeaders[name];
        },
        setHeader(name, value) {
          resHeaders[name] = value;
          return this;
        },
        getHeaderNames() {
          return Object.keys(resHeaders);
        },
        hasHeader(name) {
          return name in resHeaders;
        },
        removeHeader(name) {
          delete resHeaders[name];
        },
        getHeaders() {
          return resHeaders;
        },
        end(chunk, arg2, arg3) {
          if (typeof chunk === "string") {
            _resSendBody = chunk;
          }
          if (typeof arg2 === "function") {
            arg2();
          }
          if (typeof arg3 === "function") {
            arg3();
          }
          return this;
        },
        write(chunk, arg2, arg3) {
          if (typeof chunk === "string") {
            _resSendBody = chunk;
          }
          if (typeof arg2 === "function") {
            arg2(void 0);
          }
          if (typeof arg3 === "function") {
            arg3();
          }
          return true;
        },
        writeHead(statusCode, headers2) {
          this.statusCode = statusCode;
          if (headers2) {
            if (Array.isArray(headers2) || typeof headers2 === "string") {
              throw new TypeError("Raw headers  is not supported.");
            }
            for (const header in headers2) {
              const value = headers2[header];
              if (value !== void 0) {
                this.setHeader(
                  header,
                  value
                );
              }
            }
          }
          return this;
        }
      });
      const event = createEvent(reqProxy, resProxy);
      event.fetch = (url, fetchOptions) => fetchWithEvent(event, url, fetchOptions, {
        fetch: useNitroApp().localFetch
      });
      event.$fetch = (url, fetchOptions) => fetchWithEvent(event, url, fetchOptions, {
        fetch: globalThis.$fetch
      });
      event.context = incomingEvent.context;
      event.context.cache = {
        options: _opts
      };
      const body = await handler(event) || _resSendBody;
      const headers = event.node.res.getHeaders();
      headers.etag = String(
        headers.Etag || headers.etag || `W/"${hash(body)}"`
      );
      headers["last-modified"] = String(
        headers["Last-Modified"] || headers["last-modified"] || (/* @__PURE__ */ new Date()).toUTCString()
      );
      const cacheControl = [];
      if (opts.swr) {
        if (opts.maxAge) {
          cacheControl.push(`s-maxage=${opts.maxAge}`);
        }
        if (opts.staleMaxAge) {
          cacheControl.push(`stale-while-revalidate=${opts.staleMaxAge}`);
        } else {
          cacheControl.push("stale-while-revalidate");
        }
      } else if (opts.maxAge) {
        cacheControl.push(`max-age=${opts.maxAge}`);
      }
      if (cacheControl.length > 0) {
        headers["cache-control"] = cacheControl.join(", ");
      }
      const cacheEntry = {
        code: event.node.res.statusCode,
        headers,
        body
      };
      return cacheEntry;
    },
    _opts
  );
  return defineEventHandler(async (event) => {
    if (opts.headersOnly) {
      if (handleCacheHeaders(event, { maxAge: opts.maxAge })) {
        return;
      }
      return handler(event);
    }
    const response = await _cachedHandler(
      event
    );
    if (event.node.res.headersSent || event.node.res.writableEnded) {
      return response.body;
    }
    if (handleCacheHeaders(event, {
      modifiedTime: new Date(response.headers["last-modified"]),
      etag: response.headers.etag,
      maxAge: opts.maxAge
    })) {
      return;
    }
    event.node.res.statusCode = response.code;
    for (const name in response.headers) {
      const value = response.headers[name];
      if (name === "set-cookie") {
        event.node.res.appendHeader(
          name,
          splitCookiesString(value)
        );
      } else {
        if (value !== void 0) {
          event.node.res.setHeader(name, value);
        }
      }
    }
    return response.body;
  });
}
function cloneWithProxy(obj, overrides) {
  return new Proxy(obj, {
    get(target, property, receiver) {
      if (property in overrides) {
        return overrides[property];
      }
      return Reflect.get(target, property, receiver);
    },
    set(target, property, value, receiver) {
      if (property in overrides) {
        overrides[property] = value;
        return true;
      }
      return Reflect.set(target, property, value, receiver);
    }
  });
}
const cachedEventHandler = defineCachedEventHandler;

const inlineAppConfig = {};



const appConfig = defuFn(inlineAppConfig);

function getEnv(key, opts) {
  const envKey = snakeCase(key).toUpperCase();
  return destr(
    process.env[opts.prefix + envKey] ?? process.env[opts.altPrefix + envKey]
  );
}
function _isObject(input) {
  return typeof input === "object" && !Array.isArray(input);
}
function applyEnv(obj, opts, parentKey = "") {
  for (const key in obj) {
    const subKey = parentKey ? `${parentKey}_${key}` : key;
    const envValue = getEnv(subKey, opts);
    if (_isObject(obj[key])) {
      if (_isObject(envValue)) {
        obj[key] = { ...obj[key], ...envValue };
        applyEnv(obj[key], opts, subKey);
      } else if (envValue === void 0) {
        applyEnv(obj[key], opts, subKey);
      } else {
        obj[key] = envValue ?? obj[key];
      }
    } else {
      obj[key] = envValue ?? obj[key];
    }
    if (opts.envExpansion && typeof obj[key] === "string") {
      obj[key] = _expandFromEnv(obj[key]);
    }
  }
  return obj;
}
const envExpandRx = /{{(.*?)}}/g;
function _expandFromEnv(value) {
  return value.replace(envExpandRx, (match, key) => {
    return process.env[key] || match;
  });
}

const _inlineRuntimeConfig = {
  "app": {
    "baseURL": "/"
  },
  "nitro": {
    "routeRules": {
      "/_build/assets/**": {
        "headers": {
          "cache-control": "public, immutable, max-age=31536000"
        }
      }
    }
  }
};
const envOptions = {
  prefix: "NITRO_",
  altPrefix: _inlineRuntimeConfig.nitro.envPrefix ?? process.env.NITRO_ENV_PREFIX ?? "_",
  envExpansion: _inlineRuntimeConfig.nitro.envExpansion ?? process.env.NITRO_ENV_EXPANSION ?? false
};
const _sharedRuntimeConfig = _deepFreeze(
  applyEnv(klona(_inlineRuntimeConfig), envOptions)
);
function useRuntimeConfig(event) {
  {
    return _sharedRuntimeConfig;
  }
}
_deepFreeze(klona(appConfig));
function _deepFreeze(object) {
  const propNames = Object.getOwnPropertyNames(object);
  for (const name of propNames) {
    const value = object[name];
    if (value && typeof value === "object") {
      _deepFreeze(value);
    }
  }
  return Object.freeze(object);
}
new Proxy(/* @__PURE__ */ Object.create(null), {
  get: (_, prop) => {
    console.warn(
      "Please use `useRuntimeConfig()` instead of accessing config directly."
    );
    const runtimeConfig = useRuntimeConfig();
    if (prop in runtimeConfig) {
      return runtimeConfig[prop];
    }
    return void 0;
  }
});

const nitroAsyncContext = getContext("nitro-app", {
  asyncContext: true,
  AsyncLocalStorage: AsyncLocalStorage 
});

const config = useRuntimeConfig();
const _routeRulesMatcher = toRouteMatcher(
  createRouter({ routes: config.nitro.routeRules })
);
function createRouteRulesHandler(ctx) {
  return eventHandler((event) => {
    const routeRules = getRouteRules(event);
    if (routeRules.headers) {
      setHeaders(event, routeRules.headers);
    }
    if (routeRules.redirect) {
      let target = routeRules.redirect.to;
      if (target.endsWith("/**")) {
        let targetPath = event.path;
        const strpBase = routeRules.redirect._redirectStripBase;
        if (strpBase) {
          targetPath = withoutBase(targetPath, strpBase);
        }
        target = joinURL(target.slice(0, -3), targetPath);
      } else if (event.path.includes("?")) {
        const query = getQuery(event.path);
        target = withQuery(target, query);
      }
      return sendRedirect(event, target, routeRules.redirect.statusCode);
    }
    if (routeRules.proxy) {
      let target = routeRules.proxy.to;
      if (target.endsWith("/**")) {
        let targetPath = event.path;
        const strpBase = routeRules.proxy._proxyStripBase;
        if (strpBase) {
          targetPath = withoutBase(targetPath, strpBase);
        }
        target = joinURL(target.slice(0, -3), targetPath);
      } else if (event.path.includes("?")) {
        const query = getQuery(event.path);
        target = withQuery(target, query);
      }
      return proxyRequest(event, target, {
        fetch: ctx.localFetch,
        ...routeRules.proxy
      });
    }
  });
}
function getRouteRules(event) {
  event.context._nitro = event.context._nitro || {};
  if (!event.context._nitro.routeRules) {
    event.context._nitro.routeRules = getRouteRulesForPath(
      withoutBase(event.path.split("?")[0], useRuntimeConfig().app.baseURL)
    );
  }
  return event.context._nitro.routeRules;
}
function getRouteRulesForPath(path) {
  return defu({}, ..._routeRulesMatcher.matchAll(path).reverse());
}

function createNitroApp() {
  const config = useRuntimeConfig();
  const hooks = createHooks();
  const captureError = (error, context = {}) => {
    const promise = hooks.callHookParallel("error", error, context).catch((error_) => {
      console.error("Error while capturing another error", error_);
    });
    if (context.event && isEvent(context.event)) {
      const errors = context.event.context.nitro?.errors;
      if (errors) {
        errors.push({ error, context });
      }
      if (context.event.waitUntil) {
        context.event.waitUntil(promise);
      }
    }
  };
  const h3App = createApp({
    debug: destr(false),
    onError: (error, event) => {
      captureError(error, { event, tags: ["request"] });
      return errorHandler(error, event);
    },
    onRequest: async (event) => {
      await nitroApp.hooks.callHook("request", event).catch((error) => {
        captureError(error, { event, tags: ["request"] });
      });
    },
    onBeforeResponse: async (event, response) => {
      await nitroApp.hooks.callHook("beforeResponse", event, response).catch((error) => {
        captureError(error, { event, tags: ["request", "response"] });
      });
    },
    onAfterResponse: async (event, response) => {
      await nitroApp.hooks.callHook("afterResponse", event, response).catch((error) => {
        captureError(error, { event, tags: ["request", "response"] });
      });
    }
  });
  const router = createRouter$1({
    preemptive: true
  });
  const localCall = createCall(toNodeListener(h3App));
  const _localFetch = createFetch(localCall, globalThis.fetch);
  const localFetch = (input, init) => _localFetch(input, init).then(
    (response) => normalizeFetchResponse(response)
  );
  const $fetch = createFetch$1({
    fetch: localFetch,
    Headers: Headers$1,
    defaults: { baseURL: config.app.baseURL }
  });
  globalThis.$fetch = $fetch;
  h3App.use(createRouteRulesHandler({ localFetch }));
  h3App.use(
    eventHandler((event) => {
      event.context.nitro = event.context.nitro || { errors: [] };
      const envContext = event.node.req?.__unenv__;
      if (envContext) {
        Object.assign(event.context, envContext);
      }
      event.fetch = (req, init) => fetchWithEvent(event, req, init, { fetch: localFetch });
      event.$fetch = (req, init) => fetchWithEvent(event, req, init, {
        fetch: $fetch
      });
      event.waitUntil = (promise) => {
        if (!event.context.nitro._waitUntilPromises) {
          event.context.nitro._waitUntilPromises = [];
        }
        event.context.nitro._waitUntilPromises.push(promise);
        if (envContext?.waitUntil) {
          envContext.waitUntil(promise);
        }
      };
      event.captureError = (error, context) => {
        captureError(error, { event, ...context });
      };
    })
  );
  for (const h of handlers) {
    let handler = h.lazy ? lazyEventHandler(h.handler) : h.handler;
    if (h.middleware || !h.route) {
      const middlewareBase = (config.app.baseURL + (h.route || "/")).replace(
        /\/+/g,
        "/"
      );
      h3App.use(middlewareBase, handler);
    } else {
      const routeRules = getRouteRulesForPath(
        h.route.replace(/:\w+|\*\*/g, "_")
      );
      if (routeRules.cache) {
        handler = cachedEventHandler(handler, {
          group: "nitro/routes",
          ...routeRules.cache
        });
      }
      router.use(h.route, handler, h.method);
    }
  }
  h3App.use(config.app.baseURL, router.handler);
  {
    const _handler = h3App.handler;
    h3App.handler = (event) => {
      const ctx = { event };
      return nitroAsyncContext.callAsync(ctx, () => _handler(event));
    };
  }
  const app = {
    hooks,
    h3App,
    router,
    localCall,
    localFetch,
    captureError
  };
  return app;
}
function runNitroPlugins(nitroApp2) {
  for (const plugin of plugins) {
    try {
      plugin(nitroApp2);
    } catch (error) {
      nitroApp2.captureError(error, { tags: ["plugin"] });
      throw error;
    }
  }
}
const nitroApp = createNitroApp();
function useNitroApp() {
  return nitroApp;
}
runNitroPlugins(nitroApp);

export { mt as m, trapUnhandledNodeErrors as t, useNitroApp as u };
//# sourceMappingURL=nitro.mjs.map
