
var dataTable;
$(document).ready(function () {
    loadDataTable();
});
function loadDataTable() {
    dataTable = $('#myTable').DataTable({
        "ajax": { url: '/Admin/product/GetAll' },
        "columns": [
            { data: 'name', "width": "15%" },  // Small width for name
            { data: 'description', "width": "20%" },  // More space for description
            { data: 'brand', "width": "5%" },  // Moderate width for brand
            { data: 'weightVolume', "width": "5%" },  // Moderate width for weight/volume
            { data: 'ingredients', "width": "20%" },  // More space for ingredients
            { data: 'unitPrice', "width": "5%" },  // Moderate width for unit price
            { data: 'unitsInStock', "width": "5%" },  // Moderate width for units in stock
            { data: 'isAvailable', "width": "5%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-[100px]  text-center rounded-md flex justify-evenly">
                            <a href="/Admin/Product/Upsert/${data}" 
                               class="btn font-mono rounded-lg m-2 p-[10px] bg-green-500 hover:bg-green-300">
                                Edit
                            </a>
                            <a onClick="Delete('/Admin/Product/Delete/${data}')"
                               class="btn font-mono rounded-lg m-2 p-[10px] bg-red-500 hover:bg-red-300">
                                Delete
                            </a>
                        </div>`
                }, "width": "16%"
            }
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE', 
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            });
           
        }
    });

}