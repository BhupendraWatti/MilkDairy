var datatable;
$(document).ready(function () {

    LoadTable();
});

function LoadTable() {
    datatable = $('#mytable').DataTable({
        "ajax": { url: "/Admin/user/GetAll" },
        "columns": [
            { "data": "name", "width": '20%' },
            { "data": "email", "width": '15%' },
            { "data": "phoneNumber", "width": '20%' },
            { "data": "company.name", "width": '15%' },
            { "data": "", "width": '15%' },
           
            {
                data: 'id',
                "render": function (data) {

                    return `
                    <div class="w-[100px]  text-center rounded-md flex justify-evenly">
                            <a href="/Admin/Company/Upsert/${data}" 
                               class="btn font-mono rounded-lg m-2 p-[10px] bg-green-500 hover:bg-green-300">
                                Edit
                            </a>
                          
                    </div>
                    
                    
                    `
                }, "width": "15%"
            }
        ]
    });
}

