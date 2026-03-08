-- 更新admin用户的权限，添加用户管理权限
-- 执行此脚本为admin账号添加用户管理权限

UPDATE EP_User 
SET Permissions = '申请管理,审批管理,验收管理,对账管理,财务审核,统计报表,用户管理'
WHERE Username = 'admin';

-- 如果没有admin用户，创建一个默认的admin用户
IF NOT EXISTS (SELECT 1 FROM EP_User WHERE Username = 'admin')
BEGIN
    INSERT INTO EP_User (Username, Password, Permissions, IsActive, CreateTime)
    VALUES ('admin', 'admin', '申请管理,审批管理,验收管理,对账管理,财务审核,统计报表,用户管理', 1, GETDATE());
END

-- 查看admin用户的权限
SELECT Username, Permissions FROM EP_User WHERE Username = 'admin';
