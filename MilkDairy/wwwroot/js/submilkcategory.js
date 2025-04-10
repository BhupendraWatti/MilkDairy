var datatable;
$(document).ready(function () {
    LoadTable();
});

function LoadTable() {
    datatable = $('#myTable').DataTable({
        "ajax": { url: "/Admin/subcategoiesMilk/GetAll" },
        "columns": [
            { data: 'milkId', "width": '5%' },
            { data: 'milkType', "width": '7%' },
            { data: 'milkName', "width": '15%' },
            { data: 'fatConent', "width": '5%' },
            { data: 'proteinContent', "width": '5%' },
            { data: 'isOrganic', "width": '5%' },
            { data: 'packagingType', "width": '5%' },
            {
                data: 'id',
                "render": function (data) {
                    return `
                            <div class="w-[100px]  text-center rounded-md flex justify-evenly">
                            <a href="/Admin/subcategoies/Upsert/${data}"
                               class="btn font-mono rounded-lg m-2 p-[10px] bg-green-500 hover:bg-green-300">
                                Edit
                            </a>
                            <a onClick="Delete('/Admin/subcategoies/Delete/${data}')"
                               class="btn font-mono rounded-lg m-2 p-[10px] bg-red-500 hover:bg-red-300">
                                Delete
                            </a>
                        </div>
                    
                    `
                }, "width": "10%"
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