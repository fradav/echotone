import { Component } from 'solid-js';
import { JSX } from 'solid-js/h/jsx-runtime';
import { Lorem } from "solid-lorem";


export const Vignette: Component<{ title: string, text: JSX.Element, image: string, width: number, height: number }> = (props) => {
    return (
        <div class="vignette">
            <div class="image"
                style={{
                    'background-image': `url(${props.image})`,
                    'aspect-ratio': `${props.width}/${props.height}`
                }}>
            </div>
            <div class="over">
                <div class="hover">
                    <h3 class="sub">{props.title}</h3>
                </div>
            </div>
            <div class="alltext">
                props.text
            </div>
        </div>
    );
}