async function add(bookId) {
    try {
        let response = await fetch(`/Cart/AddItem?bookId=${bookId}`);
        if (response.status == 200) {
            let result = await response.json();
            console.log(result);
            let cartCountEl = document.getElementById("cartCount");
            cartCountEl.innerHTML = result;
        }
    }
    catch (err) {
        console.log(err);
    }
}
