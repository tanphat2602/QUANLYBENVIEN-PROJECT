package com.hospital.qlbv.service;

import com.hospital.qlbv.dto.*;
import com.hospital.qlbv.entity.*;
import com.hospital.qlbv.repository.*;
import com.hospital.qlbv.security.JwtTokenProvider;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.Authentication;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.List;
import java.util.stream.Collectors;

@Service
public class AuthService {

    private final AuthenticationManager authenticationManager;
    private final UserRepository userRepository;
    private final RoleRepository roleRepository;
    private final UserRoleRepository userRoleRepository;
    private final PatientRepository patientRepository;
    private final DoctorRepository doctorRepository;
    private final PermissionRepository permissionRepository;
    private final RolePermissionRepository rolePermissionRepository;
    private final PasswordEncoder passwordEncoder;
    private final JwtTokenProvider jwtTokenProvider;

    public AuthService(AuthenticationManager authenticationManager, UserRepository userRepository,
                      RoleRepository roleRepository, UserRoleRepository userRoleRepository,
                      PatientRepository patientRepository, DoctorRepository doctorRepository,
                      PermissionRepository permissionRepository,
                      RolePermissionRepository rolePermissionRepository,
                      PasswordEncoder passwordEncoder, JwtTokenProvider jwtTokenProvider) {
        this.authenticationManager = authenticationManager;
        this.userRepository = userRepository;
        this.roleRepository = roleRepository;
        this.userRoleRepository = userRoleRepository;
        this.patientRepository = patientRepository;
        this.doctorRepository = doctorRepository;
        this.permissionRepository = permissionRepository;
        this.rolePermissionRepository = rolePermissionRepository;
        this.passwordEncoder = passwordEncoder;
        this.jwtTokenProvider = jwtTokenProvider;
    }

    public AuthResponse login(LoginRequest request) {
        Authentication authentication = authenticationManager.authenticate(
                new UsernamePasswordAuthenticationToken(request.getUsername(), request.getPassword())
        );

        User user = userRepository.findByUsername(request.getUsername())
                .orElseThrow(() -> new RuntimeException("User not found"));

        String token = jwtTokenProvider.generateToken(request.getUsername());
        
        List<String> roles = getUserRoles(user);
        String primaryRole = roles.isEmpty() ? "PATIENT" : roles.get(0);
        List<String> permissions = getUserPermissions(roles);
        
        Integer patientId = null;
        Integer doctorId = null;
        
        if ("PATIENT".equals(primaryRole)) {
            var optPatient = patientRepository.findByUserId(user.getUserId());
            if (optPatient.isPresent()) {
                patientId = optPatient.get().getPatientId();
            }
        } else if ("DOCTOR".equals(primaryRole)) {
            var optDoctor = doctorRepository.findByUserId(user.getUserId());
            if (optDoctor.isPresent()) {
                doctorId = optDoctor.get().getDoctorId();
            }
        }

        return AuthResponse.builder()
                .token(token)
                .userId(user.getUserId())
                .username(user.getUsername())
                .fullName(user.getFullName())
                .role(primaryRole)
                .email(user.getEmail())
                .roles(roles)
                .permissions(permissions)
                .patientId(patientId)
                .doctorId(doctorId)
                .build();
    }

    @Transactional
    public AuthResponse register(RegisterRequest request) {
        if (userRepository.existsByUsername(request.getUsername())) {
            throw new RuntimeException("Username already exists");
        }
        if (userRepository.existsByEmail(request.getEmail())) {
            throw new RuntimeException("Email already exists");
        }

        User user = User.builder()
                .username(request.getUsername())
                .passwordHash(passwordEncoder.encode(request.getPassword()))
                .email(request.getEmail())
                .fullName(request.getFullName())
                .phone(request.getPhone())
                .gender(request.getGender())
                .status("Hoạt động")
                .createdAt(getCurrentDateTime())
                .build();

        user = userRepository.save(user);

        // Assign Patient role
        Role patientRole = roleRepository.findByRoleName("PATIENT")
                .orElseThrow(() -> new RuntimeException("Patient role not found"));

        UserRole userRole = UserRole.builder()
                .userId(user.getUserId())
                .roleId(patientRole.getRoleId())
                .build();
        userRoleRepository.save(userRole);

        // Create patient record
        Patient patient = Patient.builder()
                .userId(user.getUserId())
                .status("Hoạt động")
                .createdAt(getCurrentDateTime())
                .build();
        patient = patientRepository.save(patient);

        String token = jwtTokenProvider.generateToken(request.getUsername());

        return AuthResponse.builder()
                .token(token)
                .userId(user.getUserId())
                .username(user.getUsername())
                .fullName(user.getFullName())
                .role("PATIENT")
                .email(user.getEmail())
                .roles(List.of("PATIENT"))
                .permissions(List.of("PATIENT_VIEW_OWN", "PATIENT_EDIT_OWN", "PATIENT_BOOK_APPOINTMENT", 
                    "PATIENT_CANCEL_OWN", "PATIENT_VIEW_RECORD", "PATIENT_VIEW_PRESCRIPTION", 
                    "PATIENT_VIEW_PAYMENT", "PATIENT_PAY"))
                .patientId(patient.getPatientId())
                .build();
    }

    private List<String> getUserRoles(User user) {
        return user.getUserRoles().stream()
                .map(ur -> ur.getRole().getRoleName())
                .collect(Collectors.toList());
    }
    
    private List<String> getUserPermissions(List<String> roles) {
        List<Role> roleEntities = roleRepository.findByRoleNameIn(roles);
        List<Integer> roleIds = roleEntities.stream().map(Role::getRoleId).collect(Collectors.toList());
        
        return rolePermissionRepository.findAll().stream()
                .filter(rp -> roleIds.contains(rp.getRoleId()))
                .map(rp -> permissionRepository.findById(rp.getPermissionId()))
                .filter(java.util.Optional::isPresent)
                .map(java.util.Optional::get)
                .map(Permission::getPermissionName)
                .distinct()
                .collect(Collectors.toList());
    }

    private String getCurrentDateTime() {
        return LocalDateTime.now().format(DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss"));
    }
}
