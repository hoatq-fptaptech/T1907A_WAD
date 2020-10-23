
const ChangeQty = (product_id, n) => {
    let v = $("#qty-" + product_id).val();
    v = parseInt(v);
    n = parseInt(n);
    if (v + n <= 0){
        v = 1;
    } else {
        v += n;
    }
    $("#qty-" + product_id).val(v);
    AjaxData(product_id, v);
}
const UpdateQty = (e,product_id) => {
    let v = $(e).val();
    v = parseInt(v);
    if (isNaN(v)) v = 1;
    if (v <= 0) v = 1;
    $(e).val(v);
    if (event.keyCode == 13) {
        AjaxData(product_id, v);
    }
}
const AjaxData = (product_id,v) => {
    // gui ajax len change session
    $.ajax({
        method: "GET",
        url: "/Home/AjaxCart",
        data: { id: product_id, qty: v },
        success: function (rs) {
            $("#grandtotal").text(rs);
        }
    });
}