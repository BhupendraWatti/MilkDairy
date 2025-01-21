var datatable;
$(document).ready(function () {

    LoadTable();
});

function LoadTable() {
    datatable = $('#mytable').DataTable({
        "ajax": { url: "/Admin/Company/GetAll" },
        "columns": [
            { "data": "id", "width": '5%' },
            { "data": "name", "width": '10%' },
            { "data": "streetAddress", "width": '15%' },
            { "data": "state", "width": '10%' },
            { "data": "city", "width": '10%' },
            { "data": "postalCode", "width": '5%' },
            { "data": "phoneNumber", "width": '15%' },
            {
                data: 'id',
                "render": function (data) {

                    return `
                    <div class="w-[100px]  text-center rounded-md flex justify-evenly">
                            <a href="/Admin/Company/Upsert/${data}" 
                               class="btn font-mono rounded-lg m-2 p-[10px] bg-green-500 hover:bg-green-300">
                                Edit
                            </a>
                            <a onClick="Delete('/Admin/Company/Delete/${data}')"
                               class="btn font-mono rounded-lg m-2 p-[10px] bg-red-500 hover:bg-red-300">
                                Delete
                            </a>
                    </div>
                    
                    
                    `
                }, "width": "15%"
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