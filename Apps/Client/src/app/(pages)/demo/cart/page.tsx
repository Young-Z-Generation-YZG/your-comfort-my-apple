'use client';

import { useSelector } from "react-redux";
import { useAppSelector } from "~/infrastructure/redux/store";


const CartDemo = () => {

    const cartItems = useAppSelector((state) => state.cart.value.items);
    return (
        <div>
            <h1>Cart</h1>
            {cartItems.map((item, index) => (
                <div key={index}>
                    <p>{item.name}</p>
                    <p>{item.quantity}</p>
                </div>
            ))}
        </div>
    )
}

export default CartDemo;