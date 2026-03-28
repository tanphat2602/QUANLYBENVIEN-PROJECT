package com.hospital.qlbv.repository;

import com.hospital.qlbv.entity.UserRole;
import com.hospital.qlbv.entity.UserRoleId;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface UserRoleRepository extends JpaRepository<UserRole, UserRoleId> {
}
