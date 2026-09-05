class Alumni {
  final int id;
  final String fullName;
  final String email;
  final String phone;
  final int graduationYear;
  final String? departmentName;

  const Alumni({required this.id, required this.fullName, required this.email, required this.phone, required this.graduationYear, this.departmentName});
  factory Alumni.fromJson(Map<String, dynamic> json) => Alumni(
    id: json['id'] as int? ?? 0,
    fullName: json['fullName'] as String? ?? '',
    email: json['email'] as String? ?? '',
    phone: json['phone'] as String? ?? '',
    graduationYear: json['graduationYear'] as int? ?? 0,
    departmentName: json['departmentName'] as String?,
  );
}

class ClubEvent {
  final int id;
  final String title;
  final String description;
  final DateTime eventDate;
  final String? alumniName;

  const ClubEvent({required this.id, required this.title, required this.description, required this.eventDate, this.alumniName});
  factory ClubEvent.fromJson(Map<String, dynamic> json) => ClubEvent(
    id: json['id'] as int? ?? 0,
    title: json['title'] as String? ?? '',
    description: json['description'] as String? ?? '',
    eventDate: DateTime.tryParse(json['eventDate'] as String? ?? '') ?? DateTime.now(),
    alumniName: json['alumniName'] as String?,
  );
}

class JobPosting {
  final int id;
  final String jobTitle;
  final String companyName;
  final String requirements;

  const JobPosting({required this.id, required this.jobTitle, required this.companyName, required this.requirements});
  factory JobPosting.fromJson(Map<String, dynamic> json) => JobPosting(
    id: json['id'] as int? ?? 0,
    jobTitle: json['jobTitle'] as String? ?? '',
    companyName: json['companyName'] as String? ?? '',
    requirements: json['requirements'] as String? ?? '',
  );
}
