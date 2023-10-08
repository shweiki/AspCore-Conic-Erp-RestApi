using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    public partial class EditStatusDataToEqualEnumStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            // Add your custom SQL command(s) for the "Up" migration here
            string sql0 = $"update Member set Status = 1  where   Status = 0";
            migrationBuilder.Sql(sql0);


            string sql1 = $"delete from Oprationsy where TableName='Member' ";
            migrationBuilder.Sql(sql1);

            string sql2 = $"INSERT INTO Oprationsy VALUES " +
                $"('BlackList' ,'Member' ,-2 ,NULL ,'قائمة سوداء' ,'Member' ,NULL ,'danger' ,NULL ,'قائمة سوداء' ,'el-icon-finished')," +
                $"('Deactivate' ,'Member' ,-1 ,0 ,'غير نشط' ,'Member' ,NULL ,'danger' ,NULL ,'غير نشط' ,'el-icon-finished')," +
                $"('Active' ,'Member' ,1 ,0 ,'نشط' ,'Member' ,NULL ,'Info' ,NULL ,'نشط' ,'el-icon-finished')";

            migrationBuilder.Sql(sql2);


            string sql3 = $"update MembershipMovement set Status = 2 where  Status = -2";
            migrationBuilder.Sql(sql3);


            string sql4 = $"delete from Oprationsy where TableName='MembershipMovement' ";
            migrationBuilder.Sql(sql4);

            string sql5 = $"INSERT INTO Oprationsy VALUES " +
                       $"('Terminated' ,'MembershipMovement' ,-1 ,0 ,'منتهي' ,'MembershipMovement' ,NULL ,'danger' ,NULL ,'منتهي' ,'el-icon-s-release')," +
                       $"('Pending' ,'MembershipMovement' ,0 ,0 ,'قيد موافقة' ,'MembershipMovement' ,NULL ,'Info' ,NULL ,'قيد موافقة' ,'el-icon-s-release')," +
                       $"('InProgress' ,'MembershipMovement' ,1 ,0 ,'الحالي' ,'MembershipMovement' ,NULL ,'success' ,NULL ,'الحالي' ,'el-icon-s-release')," +
                       $"('Suspense' ,'MembershipMovement' ,2 ,0 ,'معلق' ,'MembershipMovement' ,NULL ,'warning' ,NULL ,'معلق' ,'el-icon-s-release')";

            migrationBuilder.Sql(sql5);



            string sql6 = $"update MembershipMovementOrder set Status = 2 where Status = -3 or Status = -2";
            migrationBuilder.Sql(sql6);


            string sql7 = $"delete from Oprationsy where TableName='MembershipMovementOrder'";
            migrationBuilder.Sql(sql7);

            string sql8 = $"INSERT INTO Oprationsy VALUES " +
                $"('Declined' ,'MembershipMovementOrder' ,-1 ,0 ,'مرفوض' ,'MembershipMovementOrder' ,NULL ,'danger' ,NULL ,'مرفوض' ,'el-icon-s-release')," +
                $"('Pending' ,'MembershipMovementOrder' ,0 ,0 ,'قيد موافقة' ,'MembershipMovementOrder' ,NULL ,'Info' ,NULL ,'قيد موافقة' ,'el-icon-s-release')," +
                $"('InProgress' ,'MembershipMovementOrder' ,1 ,0 ,'الحالي' ,'MembershipMovementOrder' ,NULL ,'success' ,NULL ,'الحالي' ,'el-icon-s-release')," +
                $"('Calculated' ,'MembershipMovementOrder' ,2 ,0 ,'تم احتساب' ,'MembershipMovementOrder' ,NULL ,'warning' ,NULL ,'تم احتساب' ,'el-icon-s-release')";
            migrationBuilder.Sql(sql8);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            // Add your custom SQL command(s) for the "Up" migration here
            string sql0 = $"update Member set Status = 0 where Status = 1";
            migrationBuilder.Sql(sql0);


            string sql1 = $"INSERT INTO Oprationsy VALUES " +
                $"('Active' ,'Member' ,1 ,NULL ,'مجمد' ,'Member' ,NULL ,'warning' ,NULL ,'مجمد' ,'el-icon-finished')," +
                $"('Active' ,'Member' ,2 ,0 ,'يوم اضافي' ,'Member' ,NULL ,'Info' ,NULL ,'يوم اضافي' ,'el-icon-finished')";

            migrationBuilder.Sql(sql1);

            string sql2 = $"update Oprationsy set Status = 0 where TableName='Member' and Status = 1";
            migrationBuilder.Sql(sql2);

            string sql3 = $"update MembershipMovement set Status = -2 where Status = 2";
            migrationBuilder.Sql(sql3);


            string sql4 = $"INSERT INTO Oprationsy VALUES " +
                $"('An extra day' ,'MembershipMovement' ,3 ,0 ,'يوم اضافي' ,'MembershipMovement' ,NULL ,'danger' ,NULL ,'يوم اضافي' ,'el-icon-s-release')";

            migrationBuilder.Sql(sql4);

            string sql5 = $"update Oprationsy set  Status = -2 where TableName='MembershipMovement' and Status = 2";
            migrationBuilder.Sql(sql5);

            string sql6 = $"update MembershipMovementOrder set Status = -2 where  Status = 2";
            migrationBuilder.Sql(sql6);


            string sql7 = $"INSERT INTO Oprationsy VALUES" +
                $" ('DoneOut' ,'MembershipMovementOrder' ,-2 ,0 ,'تم تعويض' ,'MembershipMovementOrder' ,NULL ,'danger' ,NULL ,'تم تعويض' ,'el-icon-s-release')," +
                $"('Out' ,'MembershipMovementOrder' ,2 ,0 ,'تعويض' ,'MembershipMovementOrder' ,NULL ,'success' ,NULL ,'تعويض' ,'el-icon-s-release')";

            migrationBuilder.Sql(sql7);

            string sql8 = $"update Oprationsy set Status = -3 where TableName='MembershipMovementOrder' and Status = 2";
            migrationBuilder.Sql(sql8);
        }
    }
}
