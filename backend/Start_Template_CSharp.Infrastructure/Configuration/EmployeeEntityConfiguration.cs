using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Start_Template_CSharp.Core.Entities;

namespace Start_Template_CSharp.Infrastructure.Configuration;

public class EmployeeEntityConfiguration : IEntityTypeConfiguration<EmployeeEntity>
{
    private const int MaxNameLength = 50;
    private const int MaxEmailLength = 50;
    private const int MaxPhoneLength = 50;
    public void Configure(EntityTypeBuilder<EmployeeEntity> builder)
    {
         builder.HasKey(x => x.Id);
         builder.Property(e => e.Id).IsRequired().HasValueGenerator<GuidValueGenerator>();
         builder.Property(e => e.Name).IsRequired().HasMaxLength(MaxNameLength) ;
         builder.Property(e => e.Email).IsRequired().HasMaxLength(MaxEmailLength) ;
         builder.Property(e => e.Phone).IsRequired().HasMaxLength(MaxPhoneLength) ;
         
    }
}