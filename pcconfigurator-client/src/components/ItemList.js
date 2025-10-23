import React from 'react';
import axios from 'axios';

function ItemList({ items, refresh }) {
    const handleDelete = async (id) => {
        await axios.delete(`https://localhost:7200/api/CPU/${id}`);
        refresh();
    };

    return (
        <ul>
            {items.map(item => (
                <li key={item.id}>
                    {item.name} - {item.price}$
                    <button onClick={() => handleDelete(item.id)}>Delete</button>
                </li>
            ))}
        </ul>
    );
}

export default ItemList;
