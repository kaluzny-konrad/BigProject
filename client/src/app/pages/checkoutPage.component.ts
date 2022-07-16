import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { Store } from "../services/store.service";

@Component({
    selector: "checkout-page",
    templateUrl: "checkoutPage.component.html",
    styleUrls: ['checkoutPage.component.css']
})
export class CheckoutPage {

    constructor(public store: Store, private router: Router) {
    }

    public errorMessage = "";

    onCheckout() {
        this.errorMessage = "";
        this.store.checkout().subscribe(() => {
            this.router.navigate(["/"]);
        }, err => {
            this.errorMessage = `Failed to checkout: ${err}`;
        })
    }
}