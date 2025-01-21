var datatable;
$(document).ready(function (){
    LoadTable();
});

function LoadTable() {
    dataTable = $('#mytable').DataTable({
        "ajax": { url: "/Admin/Details/GetAll" },
        "columns": [
            { data: 'id', "width": "4%" },
            { data: 'items', "width": "10%" },
            { data: 'mfd', "width": "15%" },
            { data: 'exd', "width": "15%" },
            { data: 'price', "width": "10%" },
            { data: 'quantity', "width": "5%" },
            {
                data: 'id',
                "render": function (data) {
                    return `
                            <div class="w-[100px]  text-center rounded-md flex justify-evenly">
                            <a href="/Admin/Details/Upsert/${data}" 
                               class="btn font-mono rounded-lg m-2 p-[10px] bg-green-500 hover:bg-green-300">
                                Edit
                            </a>
                            <a onClick="Delete('/Admin/Details/Delete/${data}')"
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