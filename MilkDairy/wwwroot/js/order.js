
var dataTable;
$(document).ready(function () {

    var url = window.location.search;

    if (url.includes("process")) {
        loadDataTable("process");
    }
    else {
        if (url.includes("pending")) {
            loadDataTable("pending");
        }
        else {
            if (url.includes("completed")) {
                loadDataTable("completed");
            }
            else {
                if (url.includes("approved")) {
                    loadDataTable("approved");
                }
                else {
                    loadDataTable("all");
                }
            }
        }
    }
});
function loadDataTable(status) {
    dataTable = $('#myTable').DataTable({
        "ajax": { url: '/Admin/order/GetAll?status='+ status },
        "columns": [
            { data: 'id', "width": "5%" },  // Small width for name
            { data: 'name', "width": "25%" },  // More space for description
            { data: 'phoneNumber', "width": "15%" },  // Moderate width for brand
            { data: 'applicationUser.email', "width": "15%" },  // Moderate width for weight/volume
            { data: 'orderStatus', "width": "20%" },  // More space for ingredients
            { data: 'orderTotal', "width": "5%" },  // Moderate width for unit price
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-[100px]  text-center rounded-md flex justify-evenly">
                            <a href="/Admin/order/details?orderId=${data}" 
                               class="btn font-mono rounded-lg m-2 p-[10px] bg-green-500 hover:bg-green-300">
                                Edit
                            </a>
                            
                        </div>`
                }, "width": "16%"
            }
        ]
    });
}
