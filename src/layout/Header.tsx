import { Tabs } from "@ark-ui/solid";
import { Component } from "solid-js";

const Header: Component = () => {
    return (
        <>
            <Tabs.Root defaultValue="index">
                <Tabs.List>
                    <Tabs.Trigger value="index">
                        <a href="/"><img width="199px" src="logo-full.svg" /></a>
                    </Tabs.Trigger>
                    <Tabs.Trigger value="about">
                        <a href="/about">À propos</a>
                    </Tabs.Trigger>
                    <Tabs.Trigger value="program">
                        <a href="/program">Programmation</a>
                    </Tabs.Trigger>
                    <Tabs.Trigger value="workshops">
                        <a href="/workshops">Ateliers</a>
                    </Tabs.Trigger>
                    {/* <Tabs.Trigger value="store">
        <a href="/store">Magasin</a>
      </Tabs.Trigger> */}
                    <Tabs.Trigger value="contact">
                        <a href="/contact">Contact</a>
                    </Tabs.Trigger>
                    <Tabs.Indicator />
                </Tabs.List>

            </Tabs.Root>
        </>
    )
}

export { Header };