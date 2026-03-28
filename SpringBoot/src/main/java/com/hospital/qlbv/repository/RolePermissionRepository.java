package com.hospital.qlbv.repository;

import com.hospital.qlbv.entity.RolePermission;
import com.hospital.qlbv.entity.RolePermissionId;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface RolePermissionRepository extends JpaRepository<RolePermission, RolePermissionId> {
    List<RolePermission> findByRoleId(Integer roleId);
    void deleteByRoleId(Integer roleId);
}
