-- public.admin_event_entity definition
CREATE TABLE public.admin_event_entity (
	id varchar(36) NOT NULL,
	admin_event_time int8 NULL,
	realm_id varchar(255) NULL,
	operation_type varchar(255) NULL,
	auth_realm_id varchar(255) NULL,
	auth_client_id varchar(255) NULL,
	auth_user_id varchar(255) NULL,
	ip_address varchar(255) NULL,
	resource_path varchar(2550) NULL,
	representation text NULL,
	"error" varchar(255) NULL,
	resource_type varchar(64) NULL,
	details_json text NULL,
	CONSTRAINT constraint_admin_event_entity PRIMARY KEY (id)
);
CREATE INDEX idx_admin_event_time ON public.admin_event_entity USING btree (realm_id, admin_event_time);


-- public.authenticator_config_entry definition
CREATE TABLE public.authenticator_config_entry (
	authenticator_id varchar(36) NOT NULL,
	value text NULL,
	"name" varchar(255) NOT NULL,
	CONSTRAINT constraint_auth_cfg_pk PRIMARY KEY (authenticator_id, name)
);


-- public.broker_link definition
CREATE TABLE public.broker_link (
	identity_provider varchar(255) NOT NULL,
	storage_provider_id varchar(255) NULL,
	realm_id varchar(36) NOT NULL,
	broker_user_id varchar(255) NULL,
	broker_username varchar(255) NULL,
	"token" text NULL,
	user_id varchar(255) NOT NULL,
	CONSTRAINT constr_broker_link_pk PRIMARY KEY (identity_provider, user_id)
);


-- public.client definition
CREATE TABLE public.client (
	id varchar(36) NOT NULL,
	enabled bool DEFAULT false NOT NULL,
	full_scope_allowed bool DEFAULT false NOT NULL,
	client_id varchar(255) NULL,
	not_before int4 NULL,
	public_client bool DEFAULT false NOT NULL,
	secret varchar(255) NULL,
	base_url varchar(255) NULL,
	bearer_only bool DEFAULT false NOT NULL,
	management_url varchar(255) NULL,
	surrogate_auth_required bool DEFAULT false NOT NULL,
	realm_id varchar(36) NULL,
	protocol varchar(255) NULL,
	node_rereg_timeout int4 DEFAULT 0 NULL,
	frontchannel_logout bool DEFAULT false NOT NULL,
	consent_required bool DEFAULT false NOT NULL,
	"name" varchar(255) NULL,
	service_accounts_enabled bool DEFAULT false NOT NULL,
	client_authenticator_type varchar(255) NULL,
	root_url varchar(255) NULL,
	description varchar(255) NULL,
	registration_token varchar(255) NULL,
	standard_flow_enabled bool DEFAULT true NOT NULL,
	implicit_flow_enabled bool DEFAULT false NOT NULL,
	direct_access_grants_enabled bool DEFAULT false NOT NULL,
	always_display_in_console bool DEFAULT false NOT NULL,
	CONSTRAINT constraint_7 PRIMARY KEY (id),
	CONSTRAINT uk_b71cjlbenv945rb6gcon438at UNIQUE (realm_id, client_id)
);
CREATE INDEX idx_client_id ON public.client USING btree (client_id);


-- public.client_auth_flow_bindings definition
CREATE TABLE public.client_auth_flow_bindings (
	client_id varchar(36) NOT NULL,
	flow_id varchar(36) NULL,
	binding_name varchar(255) NOT NULL,
	CONSTRAINT c_cli_flow_bind PRIMARY KEY (client_id, binding_name)
);


-- public.client_scope definition
CREATE TABLE public.client_scope (
	id varchar(36) NOT NULL,
	"name" varchar(255) NULL,
	realm_id varchar(36) NULL,
	description varchar(255) NULL,
	protocol varchar(255) NULL,
	CONSTRAINT pk_cli_template PRIMARY KEY (id),
	CONSTRAINT uk_cli_scope UNIQUE (realm_id, name)
);
CREATE INDEX idx_realm_clscope ON public.client_scope USING btree (realm_id);


-- public.client_scope_client definition
CREATE TABLE public.client_scope_client (
	client_id varchar(255) NOT NULL,
	scope_id varchar(255) NOT NULL,
	default_scope bool DEFAULT false NOT NULL,
	CONSTRAINT c_cli_scope_bind PRIMARY KEY (client_id, scope_id)
);
CREATE INDEX idx_cl_clscope ON public.client_scope_client USING btree (scope_id);
CREATE INDEX idx_clscope_cl ON public.client_scope_client USING btree (client_id);


-- public.databasechangelog definition
CREATE TABLE public.databasechangelog (
	id varchar(255) NOT NULL,
	author varchar(255) NOT NULL,
	filename varchar(255) NOT NULL,
	dateexecuted timestamp NOT NULL,
	orderexecuted int4 NOT NULL,
	exectype varchar(10) NOT NULL,
	md5sum varchar(35) NULL,
	description varchar(255) NULL,
	"comments" varchar(255) NULL,
	tag varchar(255) NULL,
	liquibase varchar(20) NULL,
	contexts varchar(255) NULL,
	labels varchar(255) NULL,
	deployment_id varchar(10) NULL
);


-- public.databasechangeloglock definition
CREATE TABLE public.databasechangeloglock (
	id int4 NOT NULL,
	"locked" bool NOT NULL,
	lockgranted timestamp NULL,
	lockedby varchar(255) NULL,
	CONSTRAINT databasechangeloglock_pkey PRIMARY KEY (id)
);


-- public.event_entity definition
CREATE TABLE public.event_entity (
	id varchar(36) NOT NULL,
	client_id varchar(255) NULL,
	details_json varchar(2550) NULL,
	"error" varchar(255) NULL,
	ip_address varchar(255) NULL,
	realm_id varchar(255) NULL,
	session_id varchar(255) NULL,
	event_time int8 NULL,
	"type" varchar(255) NULL,
	user_id varchar(255) NULL,
	details_json_long_value text NULL,
	CONSTRAINT constraint_4 PRIMARY KEY (id)
);
CREATE INDEX idx_event_time ON public.event_entity USING btree (realm_id, event_time);


-- public.fed_user_attribute definition
CREATE TABLE public.fed_user_attribute (
	id varchar(36) NOT NULL,
	"name" varchar(255) NOT NULL,
	user_id varchar(255) NOT NULL,
	realm_id varchar(36) NOT NULL,
	storage_provider_id varchar(36) NULL,
	value varchar(2024) NULL,
	long_value_hash bytea NULL,
	long_value_hash_lower_case bytea NULL,
	long_value text NULL,
	CONSTRAINT constr_fed_user_attr_pk PRIMARY KEY (id)
);
CREATE INDEX fed_user_attr_long_values ON public.fed_user_attribute USING btree (long_value_hash, name);
CREATE INDEX fed_user_attr_long_values_lower_case ON public.fed_user_attribute USING btree (long_value_hash_lower_case, name);
CREATE INDEX idx_fu_attribute ON public.fed_user_attribute USING btree (user_id, realm_id, name);


-- public.fed_user_consent definition
CREATE TABLE public.fed_user_consent (
	id varchar(36) NOT NULL,
	client_id varchar(255) NULL,
	user_id varchar(255) NOT NULL,
	realm_id varchar(36) NOT NULL,
	storage_provider_id varchar(36) NULL,
	created_date int8 NULL,
	last_updated_date int8 NULL,
	client_storage_provider varchar(36) NULL,
	external_client_id varchar(255) NULL,
	CONSTRAINT constr_fed_user_consent_pk PRIMARY KEY (id)
);
CREATE INDEX idx_fu_cnsnt_ext ON public.fed_user_consent USING btree (user_id, client_storage_provider, external_client_id);
CREATE INDEX idx_fu_consent ON public.fed_user_consent USING btree (user_id, client_id);
CREATE INDEX idx_fu_consent_ru ON public.fed_user_consent USING btree (realm_id, user_id);


-- public.fed_user_consent_cl_scope definition
CREATE TABLE public.fed_user_consent_cl_scope (
	user_consent_id varchar(36) NOT NULL,
	scope_id varchar(36) NOT NULL,
	CONSTRAINT constraint_fgrntcsnt_clsc_pm PRIMARY KEY (user_consent_id, scope_id)
);


-- public.fed_user_credential definition
CREATE TABLE public.fed_user_credential (
	id varchar(36) NOT NULL,
	salt bytea NULL,
	"type" varchar(255) NULL,
	created_date int8 NULL,
	user_id varchar(255) NOT NULL,
	realm_id varchar(36) NOT NULL,
	storage_provider_id varchar(36) NULL,
	user_label varchar(255) NULL,
	secret_data text NULL,
	credential_data text NULL,
	priority int4 NULL,
	CONSTRAINT constr_fed_user_cred_pk PRIMARY KEY (id)
);
CREATE INDEX idx_fu_credential ON public.fed_user_credential USING btree (user_id, type);
CREATE INDEX idx_fu_credential_ru ON public.fed_user_credential USING btree (realm_id, user_id);


-- public.fed_user_group_membership definition
CREATE TABLE public.fed_user_group_membership (
	group_id varchar(36) NOT NULL,
	user_id varchar(255) NOT NULL,
	realm_id varchar(36) NOT NULL,
	storage_provider_id varchar(36) NULL,
	CONSTRAINT constr_fed_user_group PRIMARY KEY (group_id, user_id)
);
CREATE INDEX idx_fu_group_membership ON public.fed_user_group_membership USING btree (user_id, group_id);
CREATE INDEX idx_fu_group_membership_ru ON public.fed_user_group_membership USING btree (realm_id, user_id);


-- public.fed_user_required_action definition
CREATE TABLE public.fed_user_required_action (
	required_action varchar(255) DEFAULT ' '::character varying NOT NULL,
	user_id varchar(255) NOT NULL,
	realm_id varchar(36) NOT NULL,
	storage_provider_id varchar(36) NULL,
	CONSTRAINT constr_fed_required_action PRIMARY KEY (required_action, user_id)
);
CREATE INDEX idx_fu_required_action ON public.fed_user_required_action USING btree (user_id, required_action);
CREATE INDEX idx_fu_required_action_ru ON public.fed_user_required_action USING btree (realm_id, user_id);


-- public.fed_user_role_mapping definition
CREATE TABLE public.fed_user_role_mapping (
	role_id varchar(36) NOT NULL,
	user_id varchar(255) NOT NULL,
	realm_id varchar(36) NOT NULL,
	storage_provider_id varchar(36) NULL,
	CONSTRAINT constr_fed_user_role PRIMARY KEY (role_id, user_id)
);
CREATE INDEX idx_fu_role_mapping ON public.fed_user_role_mapping USING btree (user_id, role_id);
CREATE INDEX idx_fu_role_mapping_ru ON public.fed_user_role_mapping USING btree (realm_id, user_id);


-- public.federated_user definition
CREATE TABLE public.federated_user (
	id varchar(255) NOT NULL,
	storage_provider_id varchar(255) NULL,
	realm_id varchar(36) NOT NULL,
	CONSTRAINT constr_federated_user PRIMARY KEY (id)
);


-- public.jgroups_ping definition
CREATE TABLE public.jgroups_ping (
	address varchar(200) NOT NULL,
	"name" varchar(200) NULL,
	cluster_name varchar(200) NOT NULL,
	ip varchar(200) NOT NULL,
	coord bool NULL,
	CONSTRAINT constraint_jgroups_ping PRIMARY KEY (address)
);


-- public.keycloak_group definition
CREATE TABLE public.keycloak_group (
	id varchar(36) NOT NULL,
	"name" varchar(255) NULL,
	parent_group varchar(36) NOT NULL,
	realm_id varchar(36) NULL,
	"type" int4 DEFAULT 0 NOT NULL,
	CONSTRAINT constraint_group PRIMARY KEY (id),
	CONSTRAINT sibling_names UNIQUE (realm_id, parent_group, name)
);


-- public.migration_model definition
CREATE TABLE public.migration_model (
	id varchar(36) NOT NULL,
	"version" varchar(36) NULL,
	update_time int8 DEFAULT 0 NOT NULL,
	CONSTRAINT constraint_migmod PRIMARY KEY (id)
);
CREATE INDEX idx_update_time ON public.migration_model USING btree (update_time);


-- public.offline_client_session definition
CREATE TABLE public.offline_client_session (
	user_session_id varchar(36) NOT NULL,
	client_id varchar(255) NOT NULL,
	offline_flag varchar(4) NOT NULL,
	"timestamp" int4 NULL,
	"data" text NULL,
	client_storage_provider varchar(36) DEFAULT 'local'::character varying NOT NULL,
	external_client_id varchar(255) DEFAULT 'local'::character varying NOT NULL,
	"version" int4 DEFAULT 0 NULL,
	CONSTRAINT constraint_offl_cl_ses_pk3 PRIMARY KEY (user_session_id, client_id, client_storage_provider, external_client_id, offline_flag)
);


-- public.offline_user_session definition
CREATE TABLE public.offline_user_session (
	user_session_id varchar(36) NOT NULL,
	user_id varchar(255) NOT NULL,
	realm_id varchar(36) NOT NULL,
	created_on int4 NOT NULL,
	offline_flag varchar(4) NOT NULL,
	"data" text NULL,
	last_session_refresh int4 DEFAULT 0 NOT NULL,
	broker_session_id varchar(1024) NULL,
	"version" int4 DEFAULT 0 NULL,
	CONSTRAINT constraint_offl_us_ses_pk2 PRIMARY KEY (user_session_id, offline_flag)
);
CREATE INDEX idx_offline_uss_by_broker_session_id ON public.offline_user_session USING btree (broker_session_id, realm_id);
CREATE INDEX idx_offline_uss_by_last_session_refresh ON public.offline_user_session USING btree (realm_id, offline_flag, last_session_refresh);
CREATE INDEX idx_offline_uss_by_user ON public.offline_user_session USING btree (user_id, realm_id, offline_flag);


-- public.org definition
CREATE TABLE public.org (
	id varchar(255) NOT NULL,
	enabled bool NOT NULL,
	realm_id varchar(255) NOT NULL,
	group_id varchar(255) NOT NULL,
	"name" varchar(255) NOT NULL,
	description varchar(4000) NULL,
	alias varchar(255) NOT NULL,
	redirect_url varchar(2048) NULL,
	CONSTRAINT "ORG_pkey" PRIMARY KEY (id),
	CONSTRAINT uk_org_alias UNIQUE (realm_id, alias),
	CONSTRAINT uk_org_group UNIQUE (group_id),
	CONSTRAINT uk_org_name UNIQUE (realm_id, name)
);


-- public.org_domain definition
CREATE TABLE public.org_domain (
	id varchar(36) NOT NULL,
	"name" varchar(255) NOT NULL,
	verified bool NOT NULL,
	org_id varchar(255) NOT NULL,
	CONSTRAINT "ORG_DOMAIN_pkey" PRIMARY KEY (id, name)
);
CREATE INDEX idx_org_domain_org_id ON public.org_domain USING btree (org_id);


-- public.realm definition
CREATE TABLE public.realm (
	id varchar(36) NOT NULL,
	access_code_lifespan int4 NULL,
	user_action_lifespan int4 NULL,
	access_token_lifespan int4 NULL,
	account_theme varchar(255) NULL,
	admin_theme varchar(255) NULL,
	email_theme varchar(255) NULL,
	enabled bool DEFAULT false NOT NULL,
	events_enabled bool DEFAULT false NOT NULL,
	events_expiration int8 NULL,
	login_theme varchar(255) NULL,
	"name" varchar(255) NULL,
	not_before int4 NULL,
	password_policy varchar(2550) NULL,
	registration_allowed bool DEFAULT false NOT NULL,
	remember_me bool DEFAULT false NOT NULL,
	reset_password_allowed bool DEFAULT false NOT NULL,
	social bool DEFAULT false NOT NULL,
	ssl_required varchar(255) NULL,
	sso_idle_timeout int4 NULL,
	sso_max_lifespan int4 NULL,
	update_profile_on_soc_login bool DEFAULT false NOT NULL,
	verify_email bool DEFAULT false NOT NULL,
	master_admin_client varchar(36) NULL,
	login_lifespan int4 NULL,
	internationalization_enabled bool DEFAULT false NOT NULL,
	default_locale varchar(255) NULL,
	reg_email_as_username bool DEFAULT false NOT NULL,
	admin_events_enabled bool DEFAULT false NOT NULL,
	admin_events_details_enabled bool DEFAULT false NOT NULL,
	edit_username_allowed bool DEFAULT false NOT NULL,
	otp_policy_counter int4 DEFAULT 0 NULL,
	otp_policy_window int4 DEFAULT 1 NULL,
	otp_policy_period int4 DEFAULT 30 NULL,
	otp_policy_digits int4 DEFAULT 6 NULL,
	otp_policy_alg varchar(36) DEFAULT 'HmacSHA1'::character varying NULL,
	otp_policy_type varchar(36) DEFAULT 'totp'::character varying NULL,
	browser_flow varchar(36) NULL,
	registration_flow varchar(36) NULL,
	direct_grant_flow varchar(36) NULL,
	reset_credentials_flow varchar(36) NULL,
	client_auth_flow varchar(36) NULL,
	offline_session_idle_timeout int4 DEFAULT 0 NULL,
	revoke_refresh_token bool DEFAULT false NOT NULL,
	access_token_life_implicit int4 DEFAULT 0 NULL,
	login_with_email_allowed bool DEFAULT true NOT NULL,
	duplicate_emails_allowed bool DEFAULT false NOT NULL,
	docker_auth_flow varchar(36) NULL,
	refresh_token_max_reuse int4 DEFAULT 0 NULL,
	allow_user_managed_access bool DEFAULT false NOT NULL,
	sso_max_lifespan_remember_me int4 DEFAULT 0 NOT NULL,
	sso_idle_timeout_remember_me int4 DEFAULT 0 NOT NULL,
	default_role varchar(255) NULL,
	CONSTRAINT constraint_4a PRIMARY KEY (id),
	CONSTRAINT uk_orvsdmla56612eaefiq6wl5oi UNIQUE (name)
);
CREATE INDEX idx_realm_master_adm_cli ON public.realm USING btree (master_admin_client);


-- public.realm_localizations definition
CREATE TABLE public.realm_localizations (
	realm_id varchar(255) NOT NULL,
	locale varchar(255) NOT NULL,
	texts text NOT NULL,
	CONSTRAINT realm_localizations_pkey PRIMARY KEY (realm_id, locale)
);


-- public.required_action_config definition
CREATE TABLE public.required_action_config (
	required_action_id varchar(36) NOT NULL,
	value text NULL,
	"name" varchar(255) NOT NULL,
	CONSTRAINT constraint_req_act_cfg_pk PRIMARY KEY (required_action_id, name)
);


-- public.resource_server definition
CREATE TABLE public.resource_server (
	id varchar(36) NOT NULL,
	allow_rs_remote_mgmt bool DEFAULT false NOT NULL,
	policy_enforce_mode int2 NOT NULL,
	decision_strategy int2 DEFAULT 1 NOT NULL,
	CONSTRAINT pk_resource_server PRIMARY KEY (id)
);


-- public.revoked_token definition
CREATE TABLE public.revoked_token (
	id varchar(255) NOT NULL,
	expire int8 NOT NULL,
	CONSTRAINT constraint_rt PRIMARY KEY (id)
);
CREATE INDEX idx_rev_token_on_expire ON public.revoked_token USING btree (expire);


-- public.user_entity definition
CREATE TABLE public.user_entity (
	id varchar(36) NOT NULL,
	email varchar(255) NULL,
	email_constraint varchar(255) NULL,
	email_verified bool DEFAULT false NOT NULL,
	enabled bool DEFAULT false NOT NULL,
	federation_link varchar(255) NULL,
	first_name varchar(255) NULL,
	last_name varchar(255) NULL,
	realm_id varchar(255) NULL,
	username varchar(255) NULL,
	created_timestamp int8 NULL,
	service_account_client_link varchar(255) NULL,
	not_before int4 DEFAULT 0 NOT NULL,
	CONSTRAINT constraint_fb PRIMARY KEY (id),
	CONSTRAINT uk_dykn684sl8up1crfei6eckhd7 UNIQUE (realm_id, email_constraint),
	CONSTRAINT uk_ru8tt6t700s9v50bu18ws5ha6 UNIQUE (realm_id, username)
);
CREATE INDEX idx_user_email ON public.user_entity USING btree (email);
CREATE INDEX idx_user_service_account ON public.user_entity USING btree (realm_id, service_account_client_link);


-- public.authentication_flow definition
CREATE TABLE public.authentication_flow (
	id varchar(36) NOT NULL,
	alias varchar(255) NULL,
	description varchar(255) NULL,
	realm_id varchar(36) NULL,
	provider_id varchar(36) DEFAULT 'basic-flow'::character varying NOT NULL,
	top_level bool DEFAULT false NOT NULL,
	built_in bool DEFAULT false NOT NULL,
	CONSTRAINT constraint_auth_flow_pk PRIMARY KEY (id),
	CONSTRAINT fk_auth_flow_realm FOREIGN KEY (realm_id) REFERENCES public.realm(id)
);
CREATE INDEX idx_auth_flow_realm ON public.authentication_flow USING btree (realm_id);


-- public.authenticator_config definition
CREATE TABLE public.authenticator_config (
	id varchar(36) NOT NULL,
	alias varchar(255) NULL,
	realm_id varchar(36) NULL,
	CONSTRAINT constraint_auth_pk PRIMARY KEY (id),
	CONSTRAINT fk_auth_realm FOREIGN KEY (realm_id) REFERENCES public.realm(id)
);
CREATE INDEX idx_auth_config_realm ON public.authenticator_config USING btree (realm_id);


-- public.client_attributes definition
CREATE TABLE public.client_attributes (
	client_id varchar(36) NOT NULL,
	"name" varchar(255) NOT NULL,
	value text NULL,
	CONSTRAINT constraint_3c PRIMARY KEY (client_id, name),
	CONSTRAINT fk3c47c64beacca966 FOREIGN KEY (client_id) REFERENCES public.client(id)
);
CREATE INDEX idx_client_att_by_name_value ON public.client_attributes USING btree (name, substr(value, 1, 255));


-- public.client_initial_access definition
CREATE TABLE public.client_initial_access (
	id varchar(36) NOT NULL,
	realm_id varchar(36) NOT NULL,
	"timestamp" int4 NULL,
	expiration int4 NULL,
	count int4 NULL,
	remaining_count int4 NULL,
	CONSTRAINT cnstr_client_init_acc_pk PRIMARY KEY (id),
	CONSTRAINT fk_client_init_acc_realm FOREIGN KEY (realm_id) REFERENCES public.realm(id)
);
CREATE INDEX idx_client_init_acc_realm ON public.client_initial_access USING btree (realm_id);


-- public.client_node_registrations definition
CREATE TABLE public.client_node_registrations (
	client_id varchar(36) NOT NULL,
	value int4 NULL,
	"name" varchar(255) NOT NULL,
	CONSTRAINT constraint_84 PRIMARY KEY (client_id, name),
	CONSTRAINT fk4129723ba992f594 FOREIGN KEY (client_id) REFERENCES public.client(id)
);


-- public.client_scope_attributes definition
CREATE TABLE public.client_scope_attributes (
	scope_id varchar(36) NOT NULL,
	value varchar(2048) NULL,
	"name" varchar(255) NOT NULL,
	CONSTRAINT pk_cl_tmpl_attr PRIMARY KEY (scope_id, name),
	CONSTRAINT fk_cl_scope_attr_scope FOREIGN KEY (scope_id) REFERENCES public.client_scope(id)
);
CREATE INDEX idx_clscope_attrs ON public.client_scope_attributes USING btree (scope_id);


-- public.client_scope_role_mapping definition
CREATE TABLE public.client_scope_role_mapping (
	scope_id varchar(36) NOT NULL,
	role_id varchar(36) NOT NULL,
	CONSTRAINT pk_template_scope PRIMARY KEY (scope_id, role_id),
	CONSTRAINT fk_cl_scope_rm_scope FOREIGN KEY (scope_id) REFERENCES public.client_scope(id)
);
CREATE INDEX idx_clscope_role ON public.client_scope_role_mapping USING btree (scope_id);
CREATE INDEX idx_role_clscope ON public.client_scope_role_mapping USING btree (role_id);


-- public.component definition
CREATE TABLE public.component (
	id varchar(36) NOT NULL,
	"name" varchar(255) NULL,
	parent_id varchar(36) NULL,
	provider_id varchar(36) NULL,
	provider_type varchar(255) NULL,
	realm_id varchar(36) NULL,
	sub_type varchar(255) NULL,
	CONSTRAINT constr_component_pk PRIMARY KEY (id),
	CONSTRAINT fk_component_realm FOREIGN KEY (realm_id) REFERENCES public.realm(id)
);
CREATE INDEX idx_component_provider_type ON public.component USING btree (provider_type);
CREATE INDEX idx_component_realm ON public.component USING btree (realm_id);


-- public.component_config definition
CREATE TABLE public.component_config (
	id varchar(36) NOT NULL,
	component_id varchar(36) NOT NULL,
	"name" varchar(255) NOT NULL,
	value text NULL,
	CONSTRAINT constr_component_config_pk PRIMARY KEY (id),
	CONSTRAINT fk_component_config FOREIGN KEY (component_id) REFERENCES public.component(id)
);
CREATE INDEX idx_compo_config_compo ON public.component_config USING btree (component_id);


-- public.credential definition
CREATE TABLE public.credential (
	id varchar(36) NOT NULL,
	salt bytea NULL,
	"type" varchar(255) NULL,
	user_id varchar(36) NULL,
	created_date int8 NULL,
	user_label varchar(255) NULL,
	secret_data text NULL,
	credential_data text NULL,
	priority int4 NULL,
	CONSTRAINT constraint_f PRIMARY KEY (id),
	CONSTRAINT fk_pfyr0glasqyl0dei3kl69r6v0 FOREIGN KEY (user_id) REFERENCES public.user_entity(id)
);
CREATE INDEX idx_user_credential ON public.credential USING btree (user_id);


-- public.default_client_scope definition
CREATE TABLE public.default_client_scope (
	realm_id varchar(36) NOT NULL,
	scope_id varchar(36) NOT NULL,
	default_scope bool DEFAULT false NOT NULL,
	CONSTRAINT r_def_cli_scope_bind PRIMARY KEY (realm_id, scope_id),
	CONSTRAINT fk_r_def_cli_scope_realm FOREIGN KEY (realm_id) REFERENCES public.realm(id)
);
CREATE INDEX idx_defcls_realm ON public.default_client_scope USING btree (realm_id);
CREATE INDEX idx_defcls_scope ON public.default_client_scope USING btree (scope_id);


-- public.federated_identity definition
CREATE TABLE public.federated_identity (
	identity_provider varchar(255) NOT NULL,
	realm_id varchar(36) NULL,
	federated_user_id varchar(255) NULL,
	federated_username varchar(255) NULL,
	"token" text NULL,
	user_id varchar(36) NOT NULL,
	CONSTRAINT constraint_40 PRIMARY KEY (identity_provider, user_id),
	CONSTRAINT fk404288b92ef007a6 FOREIGN KEY (user_id) REFERENCES public.user_entity(id)
);
CREATE INDEX idx_fedidentity_feduser ON public.federated_identity USING btree (federated_user_id);
CREATE INDEX idx_fedidentity_user ON public.federated_identity USING btree (user_id);


-- public.group_attribute definition
CREATE TABLE public.group_attribute (
	id varchar(36) DEFAULT 'sybase-needs-something-here'::character varying NOT NULL,
	"name" varchar(255) NOT NULL,
	value varchar(255) NULL,
	group_id varchar(36) NOT NULL,
	CONSTRAINT constraint_group_attribute_pk PRIMARY KEY (id),
	CONSTRAINT fk_group_attribute_group FOREIGN KEY (group_id) REFERENCES public.keycloak_group(id)
);
CREATE INDEX idx_group_att_by_name_value ON public.group_attribute USING btree (name, ((value)::character varying(250)));
CREATE INDEX idx_group_attr_group ON public.group_attribute USING btree (group_id);


-- public.group_role_mapping definition
CREATE TABLE public.group_role_mapping (
	role_id varchar(36) NOT NULL,
	group_id varchar(36) NOT NULL,
	CONSTRAINT constraint_group_role PRIMARY KEY (role_id, group_id),
	CONSTRAINT fk_group_role_group FOREIGN KEY (group_id) REFERENCES public.keycloak_group(id)
);
CREATE INDEX idx_group_role_mapp_group ON public.group_role_mapping USING btree (group_id);


-- public.identity_provider definition
CREATE TABLE public.identity_provider (
	internal_id varchar(36) NOT NULL,
	enabled bool DEFAULT false NOT NULL,
	provider_alias varchar(255) NULL,
	provider_id varchar(255) NULL,
	store_token bool DEFAULT false NOT NULL,
	authenticate_by_default bool DEFAULT false NOT NULL,
	realm_id varchar(36) NULL,
	add_token_role bool DEFAULT true NOT NULL,
	trust_email bool DEFAULT false NOT NULL,
	first_broker_login_flow_id varchar(36) NULL,
	post_broker_login_flow_id varchar(36) NULL,
	provider_display_name varchar(255) NULL,
	link_only bool DEFAULT false NOT NULL,
	organization_id varchar(255) NULL,
	hide_on_login bool DEFAULT false NULL,
	CONSTRAINT constraint_2b PRIMARY KEY (internal_id),
	CONSTRAINT uk_2daelwnibji49avxsrtuf6xj33 UNIQUE (provider_alias, realm_id),
	CONSTRAINT fk2b4ebc52ae5c3b34 FOREIGN KEY (realm_id) REFERENCES public.realm(id)
);
CREATE INDEX idx_ident_prov_realm ON public.identity_provider USING btree (realm_id);
CREATE INDEX idx_idp_for_login ON public.identity_provider USING btree (realm_id, enabled, link_only, hide_on_login, organization_id);
CREATE INDEX idx_idp_realm_org ON public.identity_provider USING btree (realm_id, organization_id);


-- public.identity_provider_config definition
CREATE TABLE public.identity_provider_config (
	identity_provider_id varchar(36) NOT NULL,
	value text NULL,
	"name" varchar(255) NOT NULL,
	CONSTRAINT constraint_d PRIMARY KEY (identity_provider_id, name),
	CONSTRAINT fkdc4897cf864c4e43 FOREIGN KEY (identity_provider_id) REFERENCES public.identity_provider(internal_id)
);


-- public.identity_provider_mapper definition
CREATE TABLE public.identity_provider_mapper (
	id varchar(36) NOT NULL,
	"name" varchar(255) NOT NULL,
	idp_alias varchar(255) NOT NULL,
	idp_mapper_name varchar(255) NOT NULL,
	realm_id varchar(36) NOT NULL,
	CONSTRAINT constraint_idpm PRIMARY KEY (id),
	CONSTRAINT fk_idpm_realm FOREIGN KEY (realm_id) REFERENCES public.realm(id)
);
CREATE INDEX idx_id_prov_mapp_realm ON public.identity_provider_mapper USING btree (realm_id);


-- public.idp_mapper_config definition
CREATE TABLE public.idp_mapper_config (
	idp_mapper_id varchar(36) NOT NULL,
	value text NULL,
	"name" varchar(255) NOT NULL,
	CONSTRAINT constraint_idpmconfig PRIMARY KEY (idp_mapper_id, name),
	CONSTRAINT fk_idpmconfig FOREIGN KEY (idp_mapper_id) REFERENCES public.identity_provider_mapper(id)
);


-- public.keycloak_role definition
CREATE TABLE public.keycloak_role (
	id varchar(36) NOT NULL,
	client_realm_constraint varchar(255) NULL,
	client_role bool DEFAULT false NOT NULL,
	description varchar(255) NULL,
	"name" varchar(255) NULL,
	realm_id varchar(255) NULL,
	client varchar(36) NULL,
	realm varchar(36) NULL,
	CONSTRAINT "UK_J3RWUVD56ONTGSUHOGM184WW2-2" UNIQUE (name, client_realm_constraint),
	CONSTRAINT constraint_a PRIMARY KEY (id),
	CONSTRAINT fk_6vyqfe4cn4wlq8r6kt5vdsj5c FOREIGN KEY (realm) REFERENCES public.realm(id)
);
CREATE INDEX idx_keycloak_role_client ON public.keycloak_role USING btree (client);
CREATE INDEX idx_keycloak_role_realm ON public.keycloak_role USING btree (realm);


-- public.protocol_mapper definition
CREATE TABLE public.protocol_mapper (
	id varchar(36) NOT NULL,
	"name" varchar(255) NOT NULL,
	protocol varchar(255) NOT NULL,
	protocol_mapper_name varchar(255) NOT NULL,
	client_id varchar(36) NULL,
	client_scope_id varchar(36) NULL,
	CONSTRAINT constraint_pcm PRIMARY KEY (id),
	CONSTRAINT fk_cli_scope_mapper FOREIGN KEY (client_scope_id) REFERENCES public.client_scope(id),
	CONSTRAINT fk_pcm_realm FOREIGN KEY (client_id) REFERENCES public.client(id)
);
CREATE INDEX idx_clscope_protmap ON public.protocol_mapper USING btree (client_scope_id);
CREATE INDEX idx_protocol_mapper_client ON public.protocol_mapper USING btree (client_id);


-- public.protocol_mapper_config definition
CREATE TABLE public.protocol_mapper_config (
	protocol_mapper_id varchar(36) NOT NULL,
	value text NULL,
	"name" varchar(255) NOT NULL,
	CONSTRAINT constraint_pmconfig PRIMARY KEY (protocol_mapper_id, name),
	CONSTRAINT fk_pmconfig FOREIGN KEY (protocol_mapper_id) REFERENCES public.protocol_mapper(id)
);


-- public.realm_attribute definition
CREATE TABLE public.realm_attribute (
	"name" varchar(255) NOT NULL,
	realm_id varchar(36) NOT NULL,
	value text NULL,
	CONSTRAINT constraint_9 PRIMARY KEY (name, realm_id),
	CONSTRAINT fk_8shxd6l3e9atqukacxgpffptw FOREIGN KEY (realm_id) REFERENCES public.realm(id)
);
CREATE INDEX idx_realm_attr_realm ON public.realm_attribute USING btree (realm_id);


-- public.realm_default_groups definition
CREATE TABLE public.realm_default_groups (
	realm_id varchar(36) NOT NULL,
	group_id varchar(36) NOT NULL,
	CONSTRAINT con_group_id_def_groups UNIQUE (group_id),
	CONSTRAINT constr_realm_default_groups PRIMARY KEY (realm_id, group_id),
	CONSTRAINT fk_def_groups_realm FOREIGN KEY (realm_id) REFERENCES public.realm(id)
);
CREATE INDEX idx_realm_def_grp_realm ON public.realm_default_groups USING btree (realm_id);


-- public.realm_enabled_event_types definition
CREATE TABLE public.realm_enabled_event_types (
	realm_id varchar(36) NOT NULL,
	value varchar(255) NOT NULL,
	CONSTRAINT constr_realm_enabl_event_types PRIMARY KEY (realm_id, value),
	CONSTRAINT fk_h846o4h0w8epx5nwedrf5y69j FOREIGN KEY (realm_id) REFERENCES public.realm(id)
);
CREATE INDEX idx_realm_evt_types_realm ON public.realm_enabled_event_types USING btree (realm_id);


-- public.realm_events_listeners definition
CREATE TABLE public.realm_events_listeners (
	realm_id varchar(36) NOT NULL,
	value varchar(255) NOT NULL,
	CONSTRAINT constr_realm_events_listeners PRIMARY KEY (realm_id, value),
	CONSTRAINT fk_h846o4h0w8epx5nxev9f5y69j FOREIGN KEY (realm_id) REFERENCES public.realm(id)
);
CREATE INDEX idx_realm_evt_list_realm ON public.realm_events_listeners USING btree (realm_id);


-- public.realm_required_credential definition
CREATE TABLE public.realm_required_credential (
	"type" varchar(255) NOT NULL,
	form_label varchar(255) NULL,
	"input" bool DEFAULT false NOT NULL,
	secret bool DEFAULT false NOT NULL,
	realm_id varchar(36) NOT NULL,
	CONSTRAINT constraint_92 PRIMARY KEY (realm_id, type),
	CONSTRAINT fk_5hg65lybevavkqfki3kponh9v FOREIGN KEY (realm_id) REFERENCES public.realm(id)
);


-- public.realm_smtp_config definition
CREATE TABLE public.realm_smtp_config (
	realm_id varchar(36) NOT NULL,
	value varchar(255) NULL,
	"name" varchar(255) NOT NULL,
	CONSTRAINT constraint_e PRIMARY KEY (realm_id, name),
	CONSTRAINT fk_70ej8xdxgxd0b9hh6180irr0o FOREIGN KEY (realm_id) REFERENCES public.realm(id)
);


-- public.realm_supported_locales definition
CREATE TABLE public.realm_supported_locales (
	realm_id varchar(36) NOT NULL,
	value varchar(255) NOT NULL,
	CONSTRAINT constr_realm_supported_locales PRIMARY KEY (realm_id, value),
	CONSTRAINT fk_supported_locales_realm FOREIGN KEY (realm_id) REFERENCES public.realm(id)
);
CREATE INDEX idx_realm_supp_local_realm ON public.realm_supported_locales USING btree (realm_id);


-- public.redirect_uris definition
CREATE TABLE public.redirect_uris (
	client_id varchar(36) NOT NULL,
	value varchar(255) NOT NULL,
	CONSTRAINT constraint_redirect_uris PRIMARY KEY (client_id, value),
	CONSTRAINT fk_1burs8pb4ouj97h5wuppahv9f FOREIGN KEY (client_id) REFERENCES public.client(id)
);
CREATE INDEX idx_redir_uri_client ON public.redirect_uris USING btree (client_id);


-- public.required_action_provider definition
CREATE TABLE public.required_action_provider (
	id varchar(36) NOT NULL,
	alias varchar(255) NULL,
	"name" varchar(255) NULL,
	realm_id varchar(36) NULL,
	enabled bool DEFAULT false NOT NULL,
	default_action bool DEFAULT false NOT NULL,
	provider_id varchar(255) NULL,
	priority int4 NULL,
	CONSTRAINT constraint_req_act_prv_pk PRIMARY KEY (id),
	CONSTRAINT fk_req_act_realm FOREIGN KEY (realm_id) REFERENCES public.realm(id)
);
CREATE INDEX idx_req_act_prov_realm ON public.required_action_provider USING btree (realm_id);


-- public.resource_server_policy definition
CREATE TABLE public.resource_server_policy (
	id varchar(36) NOT NULL,
	"name" varchar(255) NOT NULL,
	description varchar(255) NULL,
	"type" varchar(255) NOT NULL,
	decision_strategy int2 NULL,
	logic int2 NULL,
	resource_server_id varchar(36) NOT NULL,
	"owner" varchar(255) NULL,
	CONSTRAINT constraint_farsrp PRIMARY KEY (id),
	CONSTRAINT uk_frsrpt700s9v50bu18ws5ha6 UNIQUE (name, resource_server_id),
	CONSTRAINT fk_frsrpo213xcx4wnkog82ssrfy FOREIGN KEY (resource_server_id) REFERENCES public.resource_server(id)
);
CREATE INDEX idx_res_serv_pol_res_serv ON public.resource_server_policy USING btree (resource_server_id);


-- public.resource_server_resource definition
CREATE TABLE public.resource_server_resource (
	id varchar(36) NOT NULL,
	"name" varchar(255) NOT NULL,
	"type" varchar(255) NULL,
	icon_uri varchar(255) NULL,
	"owner" varchar(255) NOT NULL,
	resource_server_id varchar(36) NOT NULL,
	owner_managed_access bool DEFAULT false NOT NULL,
	display_name varchar(255) NULL,
	CONSTRAINT constraint_farsr PRIMARY KEY (id),
	CONSTRAINT uk_frsr6t700s9v50bu18ws5ha6 UNIQUE (name, owner, resource_server_id),
	CONSTRAINT fk_frsrho213xcx4wnkog82ssrfy FOREIGN KEY (resource_server_id) REFERENCES public.resource_server(id)
);
CREATE INDEX idx_res_srv_res_res_srv ON public.resource_server_resource USING btree (resource_server_id);


-- public.resource_server_scope definition
CREATE TABLE public.resource_server_scope (
	id varchar(36) NOT NULL,
	"name" varchar(255) NOT NULL,
	icon_uri varchar(255) NULL,
	resource_server_id varchar(36) NOT NULL,
	display_name varchar(255) NULL,
	CONSTRAINT constraint_farsrs PRIMARY KEY (id),
	CONSTRAINT uk_frsrst700s9v50bu18ws5ha6 UNIQUE (name, resource_server_id),
	CONSTRAINT fk_frsrso213xcx4wnkog82ssrfy FOREIGN KEY (resource_server_id) REFERENCES public.resource_server(id)
);
CREATE INDEX idx_res_srv_scope_res_srv ON public.resource_server_scope USING btree (resource_server_id);


-- public.resource_uris definition
CREATE TABLE public.resource_uris (
	resource_id varchar(36) NOT NULL,
	value varchar(255) NOT NULL,
	CONSTRAINT constraint_resour_uris_pk PRIMARY KEY (resource_id, value),
	CONSTRAINT fk_resource_server_uris FOREIGN KEY (resource_id) REFERENCES public.resource_server_resource(id)
);


-- public.role_attribute definition
CREATE TABLE public.role_attribute (
	id varchar(36) NOT NULL,
	role_id varchar(36) NOT NULL,
	"name" varchar(255) NOT NULL,
	value varchar(255) NULL,
	CONSTRAINT constraint_role_attribute_pk PRIMARY KEY (id),
	CONSTRAINT fk_role_attribute_id FOREIGN KEY (role_id) REFERENCES public.keycloak_role(id)
);
CREATE INDEX idx_role_attribute ON public.role_attribute USING btree (role_id);


-- public.scope_mapping definition
CREATE TABLE public.scope_mapping (
	client_id varchar(36) NOT NULL,
	role_id varchar(36) NOT NULL,
	CONSTRAINT constraint_81 PRIMARY KEY (client_id, role_id),
	CONSTRAINT fk_ouse064plmlr732lxjcn1q5f1 FOREIGN KEY (client_id) REFERENCES public.client(id)
);
CREATE INDEX idx_scope_mapping_role ON public.scope_mapping USING btree (role_id);


-- public.scope_policy definition
CREATE TABLE public.scope_policy (
	scope_id varchar(36) NOT NULL,
	policy_id varchar(36) NOT NULL,
	CONSTRAINT constraint_farsrsps PRIMARY KEY (scope_id, policy_id),
	CONSTRAINT fk_frsrasp13xcx4wnkog82ssrfy FOREIGN KEY (policy_id) REFERENCES public.resource_server_policy(id),
	CONSTRAINT fk_frsrpass3xcx4wnkog82ssrfy FOREIGN KEY (scope_id) REFERENCES public.resource_server_scope(id)
);
CREATE INDEX idx_scope_policy_policy ON public.scope_policy USING btree (policy_id);


-- public.user_attribute definition
CREATE TABLE public.user_attribute (
	"name" varchar(255) NOT NULL,
	value varchar(255) NULL,
	user_id varchar(36) NOT NULL,
	id varchar(36) DEFAULT 'sybase-needs-something-here'::character varying NOT NULL,
	long_value_hash bytea NULL,
	long_value_hash_lower_case bytea NULL,
	long_value text NULL,
	CONSTRAINT constraint_user_attribute_pk PRIMARY KEY (id),
	CONSTRAINT fk_5hrm2vlf9ql5fu043kqepovbr FOREIGN KEY (user_id) REFERENCES public.user_entity(id)
);
CREATE INDEX idx_user_attribute ON public.user_attribute USING btree (user_id);
CREATE INDEX idx_user_attribute_name ON public.user_attribute USING btree (name, value);
CREATE INDEX user_attr_long_values ON public.user_attribute USING btree (long_value_hash, name);
CREATE INDEX user_attr_long_values_lower_case ON public.user_attribute USING btree (long_value_hash_lower_case, name);


-- public.user_consent definition
CREATE TABLE public.user_consent (
	id varchar(36) NOT NULL,
	client_id varchar(255) NULL,
	user_id varchar(36) NOT NULL,
	created_date int8 NULL,
	last_updated_date int8 NULL,
	client_storage_provider varchar(36) NULL,
	external_client_id varchar(255) NULL,
	CONSTRAINT constraint_grntcsnt_pm PRIMARY KEY (id),
	CONSTRAINT uk_external_consent UNIQUE (client_storage_provider, external_client_id, user_id),
	CONSTRAINT uk_local_consent UNIQUE (client_id, user_id),
	CONSTRAINT fk_grntcsnt_user FOREIGN KEY (user_id) REFERENCES public.user_entity(id)
);
CREATE INDEX idx_user_consent ON public.user_consent USING btree (user_id);


-- public.user_consent_client_scope definition
CREATE TABLE public.user_consent_client_scope (
	user_consent_id varchar(36) NOT NULL,
	scope_id varchar(36) NOT NULL,
	CONSTRAINT constraint_grntcsnt_clsc_pm PRIMARY KEY (user_consent_id, scope_id),
	CONSTRAINT fk_grntcsnt_clsc_usc FOREIGN KEY (user_consent_id) REFERENCES public.user_consent(id)
);
CREATE INDEX idx_usconsent_clscope ON public.user_consent_client_scope USING btree (user_consent_id);
CREATE INDEX idx_usconsent_scope_id ON public.user_consent_client_scope USING btree (scope_id);


-- public.user_federation_provider definition
CREATE TABLE public.user_federation_provider (
	id varchar(36) NOT NULL,
	changed_sync_period int4 NULL,
	display_name varchar(255) NULL,
	full_sync_period int4 NULL,
	last_sync int4 NULL,
	priority int4 NULL,
	provider_name varchar(255) NULL,
	realm_id varchar(36) NULL,
	CONSTRAINT constraint_5c PRIMARY KEY (id),
	CONSTRAINT fk_1fj32f6ptolw2qy60cd8n01e8 FOREIGN KEY (realm_id) REFERENCES public.realm(id)
);
CREATE INDEX idx_usr_fed_prv_realm ON public.user_federation_provider USING btree (realm_id);


-- public.user_group_membership definition
CREATE TABLE public.user_group_membership (
	group_id varchar(36) NOT NULL,
	user_id varchar(36) NOT NULL,
	membership_type varchar(255) NOT NULL,
	CONSTRAINT constraint_user_group PRIMARY KEY (group_id, user_id),
	CONSTRAINT fk_user_group_user FOREIGN KEY (user_id) REFERENCES public.user_entity(id)
);
CREATE INDEX idx_user_group_mapping ON public.user_group_membership USING btree (user_id);


-- public.user_required_action definition
CREATE TABLE public.user_required_action (
	user_id varchar(36) NOT NULL,
	required_action varchar(255) DEFAULT ' '::character varying NOT NULL,
	CONSTRAINT constraint_required_action PRIMARY KEY (required_action, user_id),
	CONSTRAINT fk_6qj3w1jw9cvafhe19bwsiuvmd FOREIGN KEY (user_id) REFERENCES public.user_entity(id)
);
CREATE INDEX idx_user_reqactions ON public.user_required_action USING btree (user_id);


-- public.user_role_mapping definition
CREATE TABLE public.user_role_mapping (
	role_id varchar(255) NOT NULL,
	user_id varchar(36) NOT NULL,
	CONSTRAINT constraint_c PRIMARY KEY (role_id, user_id),
	CONSTRAINT fk_c4fqv34p1mbylloxang7b1q3l FOREIGN KEY (user_id) REFERENCES public.user_entity(id)
);
CREATE INDEX idx_user_role_mapping ON public.user_role_mapping USING btree (user_id);


-- public.web_origins definition
CREATE TABLE public.web_origins (
	client_id varchar(36) NOT NULL,
	value varchar(255) NOT NULL,
	CONSTRAINT constraint_web_origins PRIMARY KEY (client_id, value),
	CONSTRAINT fk_lojpho213xcx4wnkog82ssrfy FOREIGN KEY (client_id) REFERENCES public.client(id)
);
CREATE INDEX idx_web_orig_client ON public.web_origins USING btree (client_id);


-- public.associated_policy definition
CREATE TABLE public.associated_policy (
	policy_id varchar(36) NOT NULL,
	associated_policy_id varchar(36) NOT NULL,
	CONSTRAINT constraint_farsrpap PRIMARY KEY (policy_id, associated_policy_id),
	CONSTRAINT fk_frsr5s213xcx4wnkog82ssrfy FOREIGN KEY (associated_policy_id) REFERENCES public.resource_server_policy(id),
	CONSTRAINT fk_frsrpas14xcx4wnkog82ssrfy FOREIGN KEY (policy_id) REFERENCES public.resource_server_policy(id)
);
CREATE INDEX idx_assoc_pol_assoc_pol_id ON public.associated_policy USING btree (associated_policy_id);


-- public.authentication_execution definition
CREATE TABLE public.authentication_execution (
	id varchar(36) NOT NULL,
	alias varchar(255) NULL,
	authenticator varchar(36) NULL,
	realm_id varchar(36) NULL,
	flow_id varchar(36) NULL,
	requirement int4 NULL,
	priority int4 NULL,
	authenticator_flow bool DEFAULT false NOT NULL,
	auth_flow_id varchar(36) NULL,
	auth_config varchar(36) NULL,
	CONSTRAINT constraint_auth_exec_pk PRIMARY KEY (id),
	CONSTRAINT fk_auth_exec_flow FOREIGN KEY (flow_id) REFERENCES public.authentication_flow(id),
	CONSTRAINT fk_auth_exec_realm FOREIGN KEY (realm_id) REFERENCES public.realm(id)
);
CREATE INDEX idx_auth_exec_flow ON public.authentication_execution USING btree (flow_id);
CREATE INDEX idx_auth_exec_realm_flow ON public.authentication_execution USING btree (realm_id, flow_id);


-- public.composite_role definition
CREATE TABLE public.composite_role (
	composite varchar(36) NOT NULL,
	child_role varchar(36) NOT NULL,
	CONSTRAINT constraint_composite_role PRIMARY KEY (composite, child_role),
	CONSTRAINT fk_a63wvekftu8jo1pnj81e7mce2 FOREIGN KEY (composite) REFERENCES public.keycloak_role(id),
	CONSTRAINT fk_gr7thllb9lu8q4vqa4524jjy8 FOREIGN KEY (child_role) REFERENCES public.keycloak_role(id)
);
CREATE INDEX idx_composite ON public.composite_role USING btree (composite);
CREATE INDEX idx_composite_child ON public.composite_role USING btree (child_role);


-- public.policy_config definition
CREATE TABLE public.policy_config (
	policy_id varchar(36) NOT NULL,
	"name" varchar(255) NOT NULL,
	value text NULL,
	CONSTRAINT constraint_dpc PRIMARY KEY (policy_id, name),
	CONSTRAINT fkdc34197cf864c4e43 FOREIGN KEY (policy_id) REFERENCES public.resource_server_policy(id)
);


-- public.resource_attribute definition
CREATE TABLE public.resource_attribute (
	id varchar(36) DEFAULT 'sybase-needs-something-here'::character varying NOT NULL,
	"name" varchar(255) NOT NULL,
	value varchar(255) NULL,
	resource_id varchar(36) NOT NULL,
	CONSTRAINT res_attr_pk PRIMARY KEY (id),
	CONSTRAINT fk_5hrm2vlf9ql5fu022kqepovbr FOREIGN KEY (resource_id) REFERENCES public.resource_server_resource(id)
);


-- public.resource_policy definition
CREATE TABLE public.resource_policy (
	resource_id varchar(36) NOT NULL,
	policy_id varchar(36) NOT NULL,
	CONSTRAINT constraint_farsrpp PRIMARY KEY (resource_id, policy_id),
	CONSTRAINT fk_frsrpos53xcx4wnkog82ssrfy FOREIGN KEY (resource_id) REFERENCES public.resource_server_resource(id),
	CONSTRAINT fk_frsrpp213xcx4wnkog82ssrfy FOREIGN KEY (policy_id) REFERENCES public.resource_server_policy(id)
);
CREATE INDEX idx_res_policy_policy ON public.resource_policy USING btree (policy_id);


-- public.resource_scope definition
CREATE TABLE public.resource_scope (
	resource_id varchar(36) NOT NULL,
	scope_id varchar(36) NOT NULL,
	CONSTRAINT constraint_farsrsp PRIMARY KEY (resource_id, scope_id),
	CONSTRAINT fk_frsrpos13xcx4wnkog82ssrfy FOREIGN KEY (resource_id) REFERENCES public.resource_server_resource(id),
	CONSTRAINT fk_frsrps213xcx4wnkog82ssrfy FOREIGN KEY (scope_id) REFERENCES public.resource_server_scope(id)
);
CREATE INDEX idx_res_scope_scope ON public.resource_scope USING btree (scope_id);


-- public.resource_server_perm_ticket definition
CREATE TABLE public.resource_server_perm_ticket (
	id varchar(36) NOT NULL,
	"owner" varchar(255) NOT NULL,
	requester varchar(255) NOT NULL,
	created_timestamp int8 NOT NULL,
	granted_timestamp int8 NULL,
	resource_id varchar(36) NOT NULL,
	scope_id varchar(36) NULL,
	resource_server_id varchar(36) NOT NULL,
	policy_id varchar(36) NULL,
	CONSTRAINT constraint_fapmt PRIMARY KEY (id),
	CONSTRAINT uk_frsr6t700s9v50bu18ws5pmt UNIQUE (owner, requester, resource_server_id, resource_id, scope_id),
	CONSTRAINT fk_frsrho213xcx4wnkog82sspmt FOREIGN KEY (resource_server_id) REFERENCES public.resource_server(id),
	CONSTRAINT fk_frsrho213xcx4wnkog83sspmt FOREIGN KEY (resource_id) REFERENCES public.resource_server_resource(id),
	CONSTRAINT fk_frsrho213xcx4wnkog84sspmt FOREIGN KEY (scope_id) REFERENCES public.resource_server_scope(id),
	CONSTRAINT fk_frsrpo2128cx4wnkog82ssrfy FOREIGN KEY (policy_id) REFERENCES public.resource_server_policy(id)
);
CREATE INDEX idx_perm_ticket_owner ON public.resource_server_perm_ticket USING btree (owner);
CREATE INDEX idx_perm_ticket_requester ON public.resource_server_perm_ticket USING btree (requester);


-- public.user_federation_config definition
CREATE TABLE public.user_federation_config (
	user_federation_provider_id varchar(36) NOT NULL,
	value varchar(255) NULL,
	"name" varchar(255) NOT NULL,
	CONSTRAINT constraint_f9 PRIMARY KEY (user_federation_provider_id, name),
	CONSTRAINT fk_t13hpu1j94r2ebpekr39x5eu5 FOREIGN KEY (user_federation_provider_id) REFERENCES public.user_federation_provider(id)
);


-- public.user_federation_mapper definition
CREATE TABLE public.user_federation_mapper (
	id varchar(36) NOT NULL,
	"name" varchar(255) NOT NULL,
	federation_provider_id varchar(36) NOT NULL,
	federation_mapper_type varchar(255) NOT NULL,
	realm_id varchar(36) NOT NULL,
	CONSTRAINT constraint_fedmapperpm PRIMARY KEY (id),
	CONSTRAINT fk_fedmapperpm_fedprv FOREIGN KEY (federation_provider_id) REFERENCES public.user_federation_provider(id),
	CONSTRAINT fk_fedmapperpm_realm FOREIGN KEY (realm_id) REFERENCES public.realm(id)
);
CREATE INDEX idx_usr_fed_map_fed_prv ON public.user_federation_mapper USING btree (federation_provider_id);
CREATE INDEX idx_usr_fed_map_realm ON public.user_federation_mapper USING btree (realm_id);


-- public.user_federation_mapper_config definition
CREATE TABLE public.user_federation_mapper_config (
	user_federation_mapper_id varchar(36) NOT NULL,
	value varchar(255) NULL,
	"name" varchar(255) NOT NULL,
	CONSTRAINT constraint_fedmapper_cfg_pm PRIMARY KEY (user_federation_mapper_id, name),
	CONSTRAINT fk_fedmapper_cfg FOREIGN KEY (user_federation_mapper_id) REFERENCES public.user_federation_mapper(id)
);