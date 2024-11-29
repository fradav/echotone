// make a card component with an image and a text
//
import { Image } from "@kobalte/core/image";
import { Lorem } from "solid-lorem";
import type { Component } from "solid-js";

const Card: Component = () => {
    return (
        <div>
            <Image fallbackDelay={600}>
                <Image.Img src="https://placehold.co/400x200?text=1" />
            </Image>
            <p><Lorem count={1} /></p>
        </div>
    );
};

export default Card;
