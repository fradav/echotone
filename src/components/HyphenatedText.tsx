import { hyphenateSync as hyphenate } from "hyphen/fr";
import { Component, JSX } from "solid-js";



const RecursiveTextMap = (elem: JSX.Element, transform: (text: string) => string): JSX.Element => {
    if (typeof elem === "function") {
      elem = (elem as () => Node)();
    }
    const arr = elem as JSX.ArrayElement;
    if (arr) {
      if (Array.isArray(arr)) {
        arr.forEach(child => RecursiveTextMap(child, transform));
      } else {
        const node = elem as Node;
        if (node) {
          if (node.childNodes && node.childNodes.length > 0) {
            node.childNodes.forEach(child => RecursiveTextMap(child as JSX.Element, transform));
          } else {
            if (node.textContent) {
              node.textContent = transform(node.textContent);
            }
          }
        } 
      }
      return arr;
    }
  }
  
  export const HyphenatedText: Component<{ children: JSX.Element }> = (props) => {
    return (
      <>
        {RecursiveTextMap(props.children, hyphenate)}
      </>
    );
  };