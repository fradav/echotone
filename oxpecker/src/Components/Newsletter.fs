namespace Components

open Oxpecker.Solid

[<AutoOpen>]
module Newsletter =

    [<SolidComponent>]
    let Newsletter () : HtmlElement =
        div(id = "mc_embed_signup") {
            form(
                action =
                    "https://space.us10.list-manage.com/subscribe/post?u=0b2288db6750688a467a9eece&id=8113d0266e&f_id=00684ee4f0",
                method = "post",
                id = "mc-embedded-subscribe-form",
                name = "mc-embedded-subscribe-form",
                class' = "validate",
                target = "_self",
                novalidate = true
            ) {
                div(id = "mc_embed_signup_scroll") {
                    h3() { @"Souscrivez à notre newsletter" }
                    div(class' = "mc-field-group") {
                        label(for' = "mce-EMAIL") { @"Email" }
                        input(
                            type' = "email",
                            name = "EMAIL",
                            class' = "required email",
                            id = "mce-EMAIL",
                            required = true
                        )
                        input(
                            type' = "submit",
                            name = "subscribe",
                            id = "mc-embedded-subscribe",
                            class' = "button",
                            value = "Souscrire"
                        )
                    }
                    div(id = "mce-responses", class' = "clear foot") {
                        div(class' = "response", id = "mce-error-response", style = "display: none;")
                        div(class' = "response", id = "mce-success-response", style = "display: none;")
                    }
                    div(style = "position: absolute; left: -5000px;").attr("aria-hidden", "true") {
                        @"/* real people should not fill this in and expect good things - do not remove this or risk form bot
                                signups */"
                        input(type' = "text", name = "b_0b2288db6750688a467a9eece_8113d0266e", value = "true")
                    }
                    p() {
                        a(href = "http://eepurl.com/i5Sd86", title = "Mailchimp - email marketing made easy and fun") {
                            span() {
                                img(
                                    src =
                                        "https://digitalasset.intuit.com/render/content/dam/intuit/mc-fe/en_us/images/intuit-mc-rewards-text-light.svg",
                                    alt = "Intuit Mailchimp"
                                )
                            }
                        }
                    }
                }
            }
        }
