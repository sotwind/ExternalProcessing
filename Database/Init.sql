-- 外发加工管理系统数据库初始化脚本
-- 注意：新系统复用原项目的数据库表，仅创建用户和权限表

-- 用户表（权限直接存储在用户表中）
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'EP_User') AND type in (N'U'))
BEGIN
    CREATE TABLE EP_User (
        UserID INT PRIMARY KEY,
        Username NVARCHAR(50) NOT NULL UNIQUE,
        Password NVARCHAR(100) NOT NULL,
        Permissions NVARCHAR(500),  -- 权限列表，逗号分隔，如：外发申请,外发审批,外发验收
        IsActive BIT DEFAULT 1,
        CreateTime DATETIME DEFAULT GETDATE()
    );

    -- 初始化管理员用户 (密码: admin123，权限用逗号分隔)
    INSERT INTO EP_User (UserID, Username, Password, Permissions) VALUES
    (1, 'admin', 'admin123', '外发申请,外发审批,外发验收,外发对账,财务审核,统计报表');
    
    PRINT '用户表创建成功';
END
ELSE
BEGIN
    PRINT '用户表已存在，跳过创建';
END
